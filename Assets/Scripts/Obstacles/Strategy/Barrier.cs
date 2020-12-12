using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Barrier : MonoBehaviour
    {
        public float collisionDamage = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -= collisionDamage;
            }
        }
    }
}