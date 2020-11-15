using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class TigerShellCollision : MonoBehaviour, IShellBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                Damage(other.gameObject);
                Destroy(gameObject);
            }else if (other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }
        }
        public void Damage(GameObject other)
        {
            other.GetComponent<Player.Player>().health -= 10;
        }
    }
}
