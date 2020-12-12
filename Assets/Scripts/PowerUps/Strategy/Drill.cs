using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class Drill : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool IsAlive { get; private set; } = true;
        public float rotationSpeed = 60f;
        public float powerUpEffect = 10f;
        public int ReservedArea { get; set; }
        public Rigidbody RigidBody { get; set; }
        public PowerUpsSpawner Spawner { get; set; }

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;
            other.GetComponent<Player.Player>().health += powerUpEffect;
            Destroy(gameObject);
            Spawner.spawnAreas[ReservedArea].isActive = false;
            IsAlive = false;
        }

        public void Move()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        public bool IsActive()
        {
            return IsAlive;
        }

        public void TakeEffect()
        {
        }
    }
}