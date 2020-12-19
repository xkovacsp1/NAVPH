using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemyTiger : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 30f;
        public float nextFire = 3.0f;
        public float fireRange = 25f;
        public float collisionDamage = 30f;
        public float fireDamage = 10f;
        public float startHealth = 100f;
        public float rotationSpeed = 10f;
        public Rigidbody enemyTigerShellPrefab;
        public GameObject fireExplosion;
        public GameObject smallExplosion;
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
            if (!Target || !Agent) return;
            var distanceFromTarget = Vector3.Distance(RigidBody.position, Target.position);

            if (distanceFromTarget > fireRange)
            {
                Agent.enabled = true;
                Agent.destination = Target.position;
            }
            // do not walk only rotate to target
            else
            {
                Agent.enabled = false;
                Vector3 direction = (Target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        public void Attack()
        {
            if (!enemyTigerShellPrefab || !RigidBody || !FireTransform) return;

            FireTimer += Time.deltaTime;
            var distanceFromTarget = Vector3.Distance(RigidBody.position, Target.position);
            if (distanceFromTarget <= fireRange && FireTimer > nextFire)
            {
                FireTimer = 0;
                var enemyTigerShellObject = Instantiate(enemyTigerShellPrefab, FireTransform.position, FireTransform.rotation);
                ShowFireExplosion();
                enemyTigerShellObject.velocity = launchForce * FireTransform.forward;
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
                ShowExplosion();
                Destroy(gameObject);
            }
        }

        private void ShowFireExplosion()
        {
            if (!fireExplosion || !FireTransform) return;
            var explosion = Instantiate(fireExplosion, FireTransform.position, FireTransform.rotation);
            explosion.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(explosion.GetComponentInChildren<ParticleSystem>(),
                explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        }


        private void ShowExplosion()
        {
            if (!smallExplosion || !RigidBody) return;
            var explosion = Instantiate(smallExplosion, RigidBody.position, RigidBody.rotation);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion.gameObject, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}