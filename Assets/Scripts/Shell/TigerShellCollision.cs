using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class TigerShellCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -= 10f;
                Destroy(gameObject);
            }else if (other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }
        }
      
    }
}
