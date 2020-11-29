using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class Drill : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool isActive = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }

        public Rigidbody RigidBody { get; set; }
    

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;
            other.GetComponent<Player.Player>().health += 10;
            Destroy(gameObject);
            PowerUpsSpawner.SpawnPoints[ReservedArea].IsActive = false;
            isActive = false;
            
        }

        public void Move()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void TakeEffect()
        {
           
        }
    }
}
