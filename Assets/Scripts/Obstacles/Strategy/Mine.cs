using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Mine : MonoBehaviour
    {
        public float collisionDamage = 20f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -= collisionDamage;
                Destroy(gameObject);
            }
        }
    }
}