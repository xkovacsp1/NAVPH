using UnityEngine;

namespace Assets.Scripts.Enemy.Strategy
{
    public class EnemyTruck : MonoBehaviour, IEnemyBehaviour
    {
        public float MovementSpeed { get; } = 7f;
        public bool isActive = true;
        public int ReservedArea { get; set; }

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
        }

        public bool IsActive()
        {
            return isActive;
        }
    }
}
