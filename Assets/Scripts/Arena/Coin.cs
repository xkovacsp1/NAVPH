using UnityEngine;

namespace Assets.Scripts.Arena
{
    public class Coin : MonoBehaviour
    {
        public float rotationSpeed = 60f;
        public Rigidbody RigidBody { get; set; }
        public int coinEffect = 1;
        public AudioClip collisionSound;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;

            if (collisionSound)
            {
                AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
            }
            other.GetComponent<Player.Player>().NumberOfCollectedCoins += coinEffect;
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }
}