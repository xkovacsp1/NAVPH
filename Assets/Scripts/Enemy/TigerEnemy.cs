using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class TigerEnemy : IEnemyBehaviour
    {
        //public float AttackTime { get; }
        public GameObject GameObject { get; }
        public float TimeBetweenAttacks { get; } = 0.5f;
        public float MovementSpeed { get; } = 2f;

        public TigerEnemy(GameObject gameObject)
        {
            this.GameObject = gameObject;

        }
        public void Move()
        {
            var rigidBody = GameObject.GetComponent<Rigidbody>();
            var movement = GameObject.transform.forward * MovementSpeed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + movement);
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }
    }
}
