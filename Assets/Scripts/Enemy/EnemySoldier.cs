using Assets.Scripts.Shell;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemySoldier : MonoBehaviour, IEnemyBehaviour
    {
        public float MovementSpeed { get; } = 1f;
        public readonly float LaunchForce = 20f;  //30f
        public float NextFire { get; set; } = 3.0f;
        public bool isActive = true;

        public int ReservedArea { get; set; }

        private Transform FireTransform { get; set; }

        private void Awake()
        {
            var c = GetComponentsInChildren<Transform>();
            FireTransform = GetComponentsInChildren<Transform>()[51];
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
