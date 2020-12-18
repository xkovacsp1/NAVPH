using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Mine : MonoBehaviour
    {
        public float collisionDamage = 20f;
        public GameObject smallExplosion;

        public Rigidbody MineRigidBody { get; set; }


        private void Awake()
        {
            MineRigidBody = gameObject.GetComponent<Rigidbody>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                var player = other.GetComponent<Player.Player>();
                if (!player) return;
                player.health -= collisionDamage;
                ShowExplosion();
                Destroy(gameObject);
            }
        }

        private void ShowExplosion()
        {
            if (!smallExplosion && !MineRigidBody) return;
            var explosion = Instantiate(smallExplosion, MineRigidBody.position, MineRigidBody.rotation);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion.gameObject, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}