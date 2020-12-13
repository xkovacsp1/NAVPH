using Assets.Scripts.Enemy.Strategy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class TigerShellCollision : MonoBehaviour
    {

        public GameObject smallExplosion;
        public Rigidbody ShellRigidBody { get; set; }

        private void Awake()
        {
            ShellRigidBody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                other.GetComponent<Player.Player>().health -=
                    GameObject.FindWithTag("EnemyTiger").GetComponent<EnemyTiger>().fireDamage;
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