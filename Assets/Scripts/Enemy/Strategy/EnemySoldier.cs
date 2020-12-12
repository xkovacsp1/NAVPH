using Assets.Scripts.Shell;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float launchForce = 20f;
        public float nextFire = 3.0f;
        public bool IsAlive { get; set; } = true;
        public int ReservedArea { get; set; }
        public Transform Target { get; private set; }
        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }

        public float fireRange = 15f; //Range within target will be attacked

        public NavMeshAgent Agent { get; private set; }

        public UnityEngine.UI.Image HealthBar { get; set; }
        public float ActualHealth { get; private set; }
        public float startHealth = 100f;
        public float fireDamage = 5f;
        public EnemySpawner Spawner { get; set; }
        public GameObject enemySoldierShellPrefab;

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
            HealthBar = GetComponentsInChildren<UnityEngine.UI.Image>()[0];
            RigidBody = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform; //target the player
            FireTransform = GetComponentsInChildren<Transform>()[51];
            ActualHealth = startHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;
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
            // create shell
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && Time.time > nextFire)
            {
                nextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject =
                    Instantiate(enemySoldierShellPrefab);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;
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
    }
}