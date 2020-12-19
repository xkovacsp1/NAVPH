using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 23f;
        public float nextFire = 2.0f;
        public float fireRange = 12f;
        public float startHealth = 50f;
        public float fireDamage = 5f;
        public Rigidbody enemySoldierShellPrefab;
        public AudioClip collisionSound;
        public GameObject fireExplosion;
        public float rotationSpeed = 10f;

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
        public Animator Animator { get; set; }

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
            HealthBar = GetComponentsInChildren<UnityEngine.UI.Image>()[0];
            RigidBody = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform;
            FireTransform = GetComponentsInChildren<Transform>()[51];
            Animator = GetComponentsInChildren<Animator>()[0];
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

            Destroy(gameObject);
            if (Spawner)
                Spawner.spawnAreas[ReservedArea].isActive = false;
            IsAlive = false;
        }

        public void Move()
        {
            if (!Target || !Agent) return;
            var distanceFromTarget = Vector3.Distance(RigidBody.position, Target.position);

            if (distanceFromTarget > fireRange)
            {
                if (Animator)
                    Animator.SetInteger($"Status_walk", 1);
                Agent.enabled = true;
                Agent.destination = Target.position;
            }
            // do not walk only rotate to target
            else
            {
                if (Animator)
                    Animator.SetInteger($"Status_walk", 0);
                Agent.enabled = false;
                Vector3 direction = (Target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }
        }

        public void Attack()
        {
            if (!enemySoldierShellPrefab || !RigidBody || !FireTransform) return;

            FireTimer += Time.deltaTime;
            var distanceFromTarget = Vector3.Distance(RigidBody.position, Target.position);
            if (distanceFromTarget <= fireRange && FireTimer > nextFire)
            {
                FireTimer = 0;
                Rigidbody enemyTigerShellObject = Instantiate(enemySoldierShellPrefab, FireTransform.position, FireTransform.rotation);
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
    }
}