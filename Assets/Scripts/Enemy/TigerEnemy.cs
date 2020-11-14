using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class TigerEnemy : IEnemyBehaviour
    {
        //public float AttackTime { get; }
        private GameObject GameObject { get; }
        private float MovementSpeed { get; } = 2f;
        private readonly float LaunchForce = 1500f;  //30f
        private float NextFire { get;  set; } = 3.0f;
        private List<Rigidbody> ShootedShells { get;  } = new List<Rigidbody>();
        private Transform FireTransform { get; }

        public TigerEnemy(GameObject gameObject)
        {
            this.GameObject = gameObject;
            FireTransform = GameObject.GetComponentsInChildren<Transform>()[2];

        }
        public void Move()
        {
            var rigidBody = GameObject.GetComponent<Rigidbody>();
            var movement = GameObject.transform.forward * MovementSpeed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + movement);
        }

        public void Attack()
        {
            // create shell
            if (Time.time > NextFire)
            {
                NextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShell = Object
                    .Instantiate(Resources.Load("prefabs/EnemyTigerShell", typeof(GameObject)) as GameObject)
                    .GetComponent<Rigidbody>();
                if (!enemyTigerShell) return;
                var transform = enemyTigerShell.transform;
                transform.rotation = FireTransform.rotation;
                transform.position = FireTransform.position;
                ShootedShells.Add(enemyTigerShell);
            }
            //move all active shells
            foreach (var shell in ShootedShells)
            {
                shell.velocity = LaunchForce * Time.deltaTime * FireTransform.forward;
                //var movement = FireTransform.forward * LaunchForce * Time.deltaTime;
                //shell.MovePosition(shell.position + movement);
            }

        }

    }
}
