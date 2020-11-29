using Assets.Scripts.PowerUps;
using UnityEngine;


    public class Coin : MonoBehaviour, IPowerUpsBehaviour
    {
        public float MovementSpeed { get; } = 2f;
        public bool isActive = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }
        
     
        public Rigidbody RigidBody { get; set; }
    

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!other.gameObject.CompareTag($"Player")) return;
        //    Destroy(gameObject);
        //    EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
        //    isActive = false;
        //}

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
            throw new System.NotImplementedException();
        }
    }
