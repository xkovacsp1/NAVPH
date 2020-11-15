using Assets.Scripts.Shell;
using UnityEngine;
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

        public float range = 35f; //Range within target will be detected
        public float stop;
        public float fireRange = 25f; //Range within target will be atacked

        private Transform FireTransform { get; set; }

        private void Awake()
        {
            Target = GameObject.FindWithTag("Player").transform; //target the player
            FireTransform = GetComponentsInChildren<Transform>()[2];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"PlayerShell")) return;
            Destroy(gameObject);
            EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
            isActive = false;
        }

        public void Move()
        {
            var rigidBody = GetComponent<Rigidbody>();
            var distance = Vector3.Distance(rigidBody.position, Target.position);
            if (distance <= range)
            {
                //look
                rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation,
                    Quaternion.LookRotation(Target.position - rigidBody.position), rotationSpeed * Time.deltaTime);
                //move
                if (distance > stop)
                {
                    var movement = transform.forward * MovementSpeed * Time.deltaTime;
                    rigidBody.MovePosition(rigidBody.position + movement);
                }
            }
            else
            {
                var movement = transform.forward * MovementSpeed * Time.deltaTime;
                rigidBody.MovePosition(rigidBody.position + movement);
            }
        }

        public void Attack()
        {
            // create shell
            //if (Time.time > NextFire)
            var rigidBody = GetComponent<Rigidbody>();
            var distance = Vector3.Distance(rigidBody.position, Target.position);
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
    }
}