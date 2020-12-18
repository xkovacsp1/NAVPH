using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class PlayerShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;

        public Rigidbody ShellRigidBody { get; set; }
        public float PlayerDamage { get; set; }

        private void Awake()
        {
            ShellRigidBody = gameObject.GetComponent<Rigidbody>();
            PlayerDamage = GameObject.FindWithTag("Player").GetComponent<Player.Player>().damage;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"EnemyTiger"))
            {
                var enemyTiger = other.GetComponent<Enemy.Strategy.EnemyTiger>();
                if (!enemyTiger) return;
                enemyTiger.TakeDamage(PlayerDamage);
                ShowExplosion();
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag($"EnemySoldier"))
            {
                var enemySoldier = other.GetComponent<Enemy.Strategy.EnemySoldier>();
                if (!enemySoldier) return;
                enemySoldier.TakeDamage(PlayerDamage);
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