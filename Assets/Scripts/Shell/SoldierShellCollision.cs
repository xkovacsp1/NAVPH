using Assets.Scripts.Enemy.Strategy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class SoldierShellCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -=
                    GameObject.FindWithTag("EnemySoldier").GetComponent<EnemySoldier>().fireDamage;
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }
        }
    }
}