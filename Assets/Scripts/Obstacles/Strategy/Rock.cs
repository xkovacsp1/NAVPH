using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Rock : MonoBehaviour
    {
        public float collisionDamage = 10f;
        public AudioSource CollisionSound { get; set; }

        private void Awake()
        {
            CollisionSound = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                if (CollisionSound)
                {
                    CollisionSound.Play();
                }

                other.GetComponent<Player.Player>().health -= collisionDamage;
            }
        }
    }
}