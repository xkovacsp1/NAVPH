using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Mine : MonoBehaviour
    {
        public float collisionDamage = 20f;
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
                if (!other.GetComponent<Player.Player>()) return;
                other.GetComponent<Player.Player>().health -= collisionDamage;
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