using UnityEngine;

namespace Assets.Scripts.Arena
{
    public class Coin : MonoBehaviour
    {
      //  public bool isActive = true;
        public float rotationSpeed = 60f;
       // public int ReservedArea { get; set; }
        public Rigidbody RigidBody { get; set; }
       // public PowerUpsSpawner Spawner { get; set; }

        private void Awake()
        {
          //  Spawner = GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;
            other.GetComponent<Player.Player>().numberOfCollectedCoins += 1;
            Destroy(gameObject);
          //  Spawner.SpawnPoints[ReservedArea].IsActive = false;
          //  isActive = false;
            
        }

        private void FixedUpdate()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

    }
}
