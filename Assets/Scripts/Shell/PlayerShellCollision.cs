using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class PlayerShellCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"EnemyTiger") /*other.gameObject.CompareTag($"Plane")*/)
            {
                other.GetComponent<Enemy.Strategy.TigerEnemy>().TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                Destroy(gameObject);

            }else if(other.gameObject.CompareTag($"EnemySoldier"))
            {
                other.GetComponent<Enemy.Strategy.EnemySoldier>().TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                Destroy(gameObject);

            }
            else if(other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);

            }

        }
     
        
    }
}
