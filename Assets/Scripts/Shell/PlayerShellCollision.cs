using UnityEngine;

namespace Assets.Scripts.Shell
{

    public class PlayerShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;
        public Rigidbody ShellRigidBody { get; set; }
        
        private void Awake()
        {
            ShellRigidBody= gameObject.GetComponent<Rigidbody>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"EnemyTiger"))
            {
                other.GetComponent<Enemy.Strategy.EnemyTiger>()
                    .TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                ShowExplosion();
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"EnemySoldier"))
            {
                other.GetComponent<Enemy.Strategy.EnemySoldier>()
                    .TakeDamage(GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage);
                ShowExplosion();
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"Plane"))
            {
                ShowExplosion();
                Destroy(gameObject);
            }
        }

        private void ShowExplosion()
        {
            if (!smallExplosion) return;
            var explosion = Instantiate(smallExplosion);
            var explosionRigidBody = explosion.GetComponent<Rigidbody>();
            explosionRigidBody.position = ShellRigidBody.position;
            explosionRigidBody.rotation = ShellRigidBody.rotation;
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion.gameObject, explosion.GetComponent<ParticleSystem>().main.duration);
        }


    }
}

