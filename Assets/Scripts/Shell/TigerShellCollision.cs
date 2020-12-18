using Assets.Scripts.Enemy.Strategy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class TigerShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;
        public Rigidbody ShellRigidBody { get; set; }
        public float TigerDamage { get; set; }

        private void Awake()
        {
            ShellRigidBody = gameObject.GetComponent<Rigidbody>();
            TigerDamage = GameObject.FindWithTag("EnemyTiger").GetComponent<EnemyTiger>().fireDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                var player = other.GetComponent<Player.Player>();
                if (!player) return;
                player.health -= TigerDamage;
                ShowExplosion();
                Destroy(gameObject);
            }
            else
            {
                ShowExplosion();
                Destroy(gameObject);
            }
        }

        private void ShowExplosion()
        {
            if (!smallExplosion || !ShellRigidBody) return;
            var explosion = Instantiate(smallExplosion);
            var explosionRigidBody = explosion.GetComponent<Rigidbody>();
            explosionRigidBody.position = ShellRigidBody.position;
            explosionRigidBody.rotation = ShellRigidBody.rotation;
            explosion.GetComponent<ParticleSystem>().Play();

            Destroy(explosion.gameObject, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}