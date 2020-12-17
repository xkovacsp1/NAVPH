using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 20f;
        public float nextFire = 3.0f;
        public float fireRange = 12f;
        public float startHealth = 100f;
        public float fireDamage = 5f;
        public GameObject enemySoldierShellPrefab;
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
            var distance = Vector3.Distance(RigidBody.position, Target.position);

            if (distance > fireRange)
            {
                if (Animator)
                    Animator.SetInteger($"Status_walk", 1);
                Agent.enabled = true;
                Agent.destination = Target.position;
            }
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
            if (!enemySoldierShellPrefab && !RigidBody) return;

            FireTimer += Time.deltaTime;
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && FireTimer > nextFire)
            {
                FireTimer = 0;
                var enemyTigerShellObject =
                    Instantiate(enemySoldierShellPrefab);
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