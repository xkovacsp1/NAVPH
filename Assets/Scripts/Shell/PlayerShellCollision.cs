using UnityEngine;

namespace Assets.Scripts.Shell
{

    public class PlayerShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"EnemyTiger"))
            {
                other.GetComponent<Enemy.Strategy.EnemyTiger>()
                    .TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                var explosion = Instantiate(smallExplosion);
                var explosionRigidBody = explosion.GetComponent<Rigidbody>();
                var shellRigidBody= gameObject.GetComponent<Rigidbody>();
                explosionRigidBody.position = shellRigidBody.position;
                explosionRigidBody.rotation = shellRigidBody.rotation;
                explosion.GetComponent<ParticleSystem>().Play();

                // Once the particles have finished, destroy the gameobject they are on.
               // Destroy(ExplosionParticle.gameObject, ExplosionParticle.main.duration);


                Destroy(gameObject); /*, ExplosionParticle.main.duration);*/
            }
            else if (other.gameObject.CompareTag($"EnemySoldier"))
            {
                other.GetComponent<Enemy.Strategy.EnemySoldier>()
                    .TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }
        }
    }
}

