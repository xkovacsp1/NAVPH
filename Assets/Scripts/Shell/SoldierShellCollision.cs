using Assets.Scripts.Enemy.Strategy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class SoldierShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;

        public Rigidbody ShellRigidBody { get; set; }

        private void Awake()
        {
            ShellRigidBody = gameObject.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"PlayerCollider"))
            {
                var player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
                if (!player) return;
                player.health -= GameObject.FindWithTag("EnemySoldier").GetComponent<EnemySoldier>().fireDamage;
                ShowExplosion();
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"Plane") || other.gameObject.CompareTag($"LeftWall") ||
                     other.gameObject.CompareTag($"RightWall"))
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