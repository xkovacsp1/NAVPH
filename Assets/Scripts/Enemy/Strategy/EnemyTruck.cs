using UnityEngine;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemyTruck : MonoBehaviour, IEnemyBehaviour
    {
        public float MovementSpeed { get; } = 7f;
        public bool isActive = true;
        public int ReservedArea { get; set; }
        public EnemySpawner Spawner { get; set; }

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"PlayerShell")) return;
            Destroy(gameObject);
            Spawner.spawnAreas[ReservedArea].isActive = false;
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
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void TakeDamage(float damage)
        {
        }
    }
}