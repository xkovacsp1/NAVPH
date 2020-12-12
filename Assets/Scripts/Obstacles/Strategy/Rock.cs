using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Rock : MonoBehaviour
    {
        public float collisionDamage = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -= collisionDamage;
            }
        }
    }
}