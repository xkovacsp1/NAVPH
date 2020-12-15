using Assets.Scripts.Shell;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 20f;
        public float nextFire = 3.0f;
        public float fireRange = 15f; //Range within target will be attacked
        public float startHealth = 100f;
        public float fireDamage = 5f;
        public GameObject enemySoldierShellPrefab;
        public AudioClip collisionSound;
        public GameObject fireExplosion;


        public float FireTimer { get; set; }
        public bool IsAlive { get; set; } = true;
        public int ReservedArea { get; set; }
        public Transform Target { get; private set; }
        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }
        public NavMeshAgent Agent { get; private set; }
        public UnityEngine.UI.Image HealthBar { get; set; }
        public float ActualHealth { get; private set; }
        public EnemySpawner Spawner { get; set; }
        
        private void Awake()
        {
            Spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
            HealthBar = GetComponentsInChildren<UnityEngine.UI.Image>()[0];
            RigidBody = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform;
            FireTransform = GetComponentsInChildren<Transform>()[51];
            ActualHealth = startHealth;
            FireTimer = nextFire;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;

            if (collisionSound)
            {
                AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
            }

            Destroy(gameObject);
            Spawner.spawnAreas[ReservedArea].isActive = false;
            IsAlive = false;
        }

        public void Move()
        {
            Agent.destination = Target.position;
        }

        public void Attack()
        {
            FireTimer += Time.deltaTime;
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && FireTimer > nextFire)
            {
                FireTimer = 0;
                nextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject =
                    Instantiate(enemySoldierShellPrefab);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;

                ShowFireExplosion();

                enemyTigerShellRigidBody.velocity = launchForce * FireTransform.forward;
                enemyTigerShellObject.AddComponent<SoldierShellCollision>();
            }
        }

        public bool IsActive()
        {
            return IsAlive;
        }

        public void TakeDamage(float damage)
        {
            ActualHealth = ActualHealth - damage;
            HealthBar.fillAmount = ActualHealth / startHealth;

            if (ActualHealth <= 0.0)
            {
                IsAlive = false;
                Spawner.spawnAreas[ReservedArea].isActive = false;
                Destroy(gameObject);
            }
        }


        private void ShowFireExplosion()
        {
            if (!fireExplosion) return;
            var explosion = Instantiate(fireExplosion);
            var explosionRigidBody = explosion.GetComponent<Rigidbody>();
            explosionRigidBody.position = FireTransform.position;
            explosionRigidBody.rotation = FireTransform.rotation;
            explosion.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(explosion.GetComponentInChildren<ParticleSystem>(), explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        }


    }
}