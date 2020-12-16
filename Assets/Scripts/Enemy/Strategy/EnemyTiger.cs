using Assets.Scripts.Shell;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemyTiger : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 30f;
        public float nextFire = 5.0f;
        public float fireRange = 25f;
        public float collisionDamage = 30f;
        public float fireDamage = 10f;
        public float startHealth = 100f;
        public GameObject enemyTigerShellPrefab;
        public GameObject fireExplosion;
        public AudioClip collisionSound;

        public float FireTimer { get; set; }
        public bool IsAlive { get; private set; } = true;
        public Transform Target { get; private set; }
        public int ReservedArea { get; set; }
        public NavMeshAgent Agent { get; private set; }

        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }
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
            FireTransform = GetComponentsInChildren<Transform>()[2];
            ActualHealth = startHealth;
            FireTimer = nextFire;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;


            if (collisionSound && RigidBody)
            {
                AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
            }


            if (HealthBar)
                HealthBar.fillAmount = ActualHealth / startHealth;

            var player = other.GetComponent<Player.Player>();
            if (!player) return;
            ActualHealth -= player.collisionDamage;
            player.health -= collisionDamage;

            if (ActualHealth <= 0.0)
            {
                IsAlive = false;
                if (Spawner)
                    Spawner.spawnAreas[ReservedArea].isActive = false;
                Destroy(gameObject);
            }
        }

        public void Move()
        {
            if (Target && Agent)
                Agent.destination = Target.position;
        }

        public void Attack()
        {
            if (!enemyTigerShellPrefab && !RigidBody) return;

            FireTimer += Time.deltaTime;
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && FireTimer > nextFire)
            {
                FireTimer = 0;
                var enemyTigerShellObject = Instantiate(enemyTigerShellPrefab);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody || !FireTransform) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;

                ShowFireExplosion();

                enemyTigerShellRigidBody.velocity = launchForce * FireTransform.forward;
            }
        }

        public bool IsActive()
        {
            return IsAlive;
        }

        public void TakeDamage(float damage)
        {
            ActualHealth -= damage;
            if (HealthBar)
                HealthBar.fillAmount = ActualHealth / startHealth;

            if (ActualHealth <= 0.0)
            {
                IsAlive = false;

                if (Spawner)
                    Spawner.spawnAreas[ReservedArea].isActive = false;

                Destroy(gameObject);
            }
        }

        private void ShowFireExplosion()
        {
            if (!fireExplosion) return;
            var explosion = Instantiate(fireExplosion);
            var explosionRigidBody = explosion.GetComponent<Rigidbody>();
            if (!explosionRigidBody || !FireTransform) return;
            explosionRigidBody.position = FireTransform.position;
            explosionRigidBody.rotation = FireTransform.rotation;
            explosion.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(explosion.GetComponentInChildren<ParticleSystem>(),
                explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        }
    }
}