using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class SoldierShellCollision : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -= 5;
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }
        }

     
    }
}
