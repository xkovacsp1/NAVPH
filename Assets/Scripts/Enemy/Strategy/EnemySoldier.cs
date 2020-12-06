using Assets.Scripts.Shell;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float MovementSpeed { get; } = 2f;
        public readonly float LaunchForce = 20f; //30f
        public float NextFire { get; set; } = 3.0f;
        public bool isActive = true;
        public int rotationSpeed = 3;
        public int ReservedArea { get; set; }
        public Transform Target { get; private set; }
        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }

        public float range = 20f; //Range within target will be detected
        public float stop;
        public float fireRange = 15f; //Range within target will be atacked

        public NavMeshAgent Agent { get; private set; }

        public UnityEngine.UI.Image HealthBar { get; set; }
        public float ActualHealth { get; private set; } = 100f;
        public float startHealth = 100f;
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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;
            Destroy(gameObject);
            Spawner.SpawnPoints[ReservedArea].IsActive = false;
            isActive = false;
        }

        public void Move()
        {
            Agent.destination = Target.position;
            /*var rigidBody = GetComponent<Rigidbody>();
            var distance = Vector3.Distance(rigidBody.position, Target.position);
            if (distance <= range)
            {
                //look
                //rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation,
                   // Quaternion.LookRotation(Target.position - rigidBody.position), rotationSpeed * Time.deltaTime);
                //move   - maybe this stop in redundant
                if (distance > stop)
                {
                    Agent.destination = Target.position;
                    // var movement = transform.forward * MovementSpeed * Time.deltaTime;
                    // rigidBody.MovePosition(rigidBody.position + movement);
                }
            }
            else
            {
                var movement = transform.forward * MovementSpeed * Time.deltaTime;
                rigidBody.MovePosition(rigidBody.position + movement);
            }*/
        }

        public void Attack()
        {
            // create shell
            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && Time.time > NextFire)
            {
                NextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject =
                    Instantiate(enemySoldierShellPrefab);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;
                enemyTigerShellRigidBody.velocity = LaunchForce * FireTransform.forward;
                enemyTigerShellObject.AddComponent<SoldierShellCollision>();
            }
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void TakeDamage(float damage)
        {
            ActualHealth = ActualHealth - damage;
            HealthBar.fillAmount = ActualHealth / startHealth;

            if (ActualHealth <= 0.0)
            {
                isActive = false;
                Spawner.SpawnPoints[ReservedArea].IsActive = false;
                Destroy(gameObject);
            }
        }
    }
}