using Assets.Scripts.Shell;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemyTiger : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 30f; //30f
        public float nextFire = 5.0f;
        public bool IsAlive { get; private set; } = true;
        public Transform Target { get; private set; }
        public int ReservedArea { get; set; }
        public float fireRange = 25f; //Range within target will be atacked
        public float collisionDamage = 30f;
        public float fireDamage = 10f;
        public NavMeshAgent Agent { get; private set; }

        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }
        public UnityEngine.UI.Image HealthBar { get; set; }
        public float ActualHealth { get; private set; }
        public float startHealth = 100f;
        public EnemySpawner Spawner { get; set; }

        public GameObject enemyTigerShellPrefab;
        public GameObject fireExplosion;


        private void Awake()
        {
            Spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
            HealthBar = GetComponentsInChildren<UnityEngine.UI.Image>()[0];
            RigidBody = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform; //target the player
            FireTransform = GetComponentsInChildren<Transform>()[2];
            ActualHealth = startHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;


            ActualHealth = ActualHealth - 10.0f;
            HealthBar.fillAmount = ActualHealth / startHealth;

            // decrease player health
            other.GetComponent<Player.Player>().health -= collisionDamage;

            if (ActualHealth <= 0.0)
            {
                IsAlive = false;
                Spawner.spawnAreas[ReservedArea].isActive = false;
                Destroy(gameObject);
            }
        }

        public void Move()
        {
            Agent.destination = Target.position;
        }

        public void Attack()
        {
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && Time.time > nextFire)
            {
                nextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject =
                    Instantiate(enemyTigerShellPrefab);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;

                ShowFireExplosion();

                enemyTigerShellRigidBody.velocity = launchForce * FireTransform.forward;
                enemyTigerShellObject.AddComponent<TigerShellCollision>();
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