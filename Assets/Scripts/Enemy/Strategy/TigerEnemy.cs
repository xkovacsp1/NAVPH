using Assets.Scripts.Shell;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemy.Strategy
{
    public class TigerEnemy : MonoBehaviour, IEnemyBehaviour
    {
        //public float AttackTime { get; }
        public float MovementSpeed { get; } = 5f;
        public readonly float LaunchForce = 30f; //30f
        public float NextFire { get; set; } = 5.0f;
        public bool isActive = true;
        public int rotationSpeed = 3;
        public Transform Target { get; private set; }
        public int ReservedArea { get; set; }

        public float range = 20f; //Range within target will be detected
        public float stop;
        public float fireRange = 25f; //Range within target will be atacked

        public NavMeshAgent Agent { get; private set; }

        private Transform FireTransform { get; set; }
        public Rigidbody RigidBody { get; set; }
        public UnityEngine.UI.Image HealthBar { get; set; }
        public float actualHealth = 100f;
        public float startHealth = 100f;

        private void Awake()
        {
            var c = GetComponentsInChildren<Transform>();
            var g = transform.GetChild(2).GetChild(0);
            HealthBar = GetComponentsInChildren<UnityEngine.UI.Image>()[0];
            RigidBody = GetComponent<Rigidbody>();
            Agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindWithTag("Player").transform; //target the player
            FireTransform = GetComponentsInChildren<Transform>()[2];
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!other.gameObject.CompareTag($"PlayerShell")) return;
        //    Destroy(gameObject);
        //    EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
        //    isActive = false;
        //}

        public void Move()
        {
            Agent.destination = Target.position;
            /* var rigidBody = GetComponent<Rigidbody>();
             var distance = Vector3.Distance(rigidBody.position, Target.position);
             if (distance <= range)
             {
                 //look
                 //rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation,
                     //Quaternion.LookRotation(Target.position - rigidBody.position), rotationSpeed * Time.deltaTime);
                 //move
                 if (distance > stop)
                 {
                     Agent.destination = Target.position;
                     //var movement = transform.forward * MovementSpeed * Time.deltaTime;
                     //rigidBody.MovePosition(rigidBody.position + movement);
 
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
            //if (Time.time > NextFire)

            var distance = Vector3.Distance(RigidBody.position, Target.position);
            if (distance <= fireRange && Time.time > NextFire)
            {
                NextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject =
                    Instantiate(Resources.Load("prefabs/EnemyTigerShell", typeof(GameObject)) as GameObject);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;
                enemyTigerShellRigidBody.velocity = LaunchForce * FireTransform.forward;
                enemyTigerShellObject.AddComponent<TigerShellCollision>();
            }
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void TakeDamage(float damage)
        {
            actualHealth = actualHealth - damage;
            HealthBar.fillAmount = actualHealth / startHealth;

            if (actualHealth <= 0.0)
            {
                isActive = false;
                EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
                Destroy(gameObject);
            }
        }
    }
}