using Assets.Scripts.Shell;
using UnityEngine;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float MovementSpeed { get; } = 1f;
        public readonly float LaunchForce = 20f;  //30f
        public float NextFire { get; set; } = 3.0f;
        public bool isActive = true;
        public int rotationSpeed = 3;
        public int ReservedArea { get; set; }
        public Transform Target { get; private set; }
        private Transform FireTransform { get; set; }

        private void Awake()
        {
            Target = GameObject.FindWithTag("Player").transform; //target the player
            FireTransform = GetComponentsInChildren<Transform>()[51];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"PlayerShell") && !other.gameObject.CompareTag($"Player")) return;
            Destroy(gameObject);
            EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
            isActive = false;

        }


        public void Move()
        {
            var rigidBody = GetComponent<Rigidbody>();
            //look
            rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation,
                Quaternion.LookRotation(Target.position - rigidBody.position), rotationSpeed * Time.deltaTime);
            var movement = transform.forward * MovementSpeed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + movement);
        }

        public void Attack()
        {
            // create shell
            if (Time.time > NextFire)
            {
                NextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject = Instantiate(Resources.Load("prefabs/EnemyTigerShell", typeof(GameObject)) as GameObject);
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
    }
}
