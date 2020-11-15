using Assets.Scripts.Shell;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemy
{
    public class TigerEnemy :MonoBehaviour, IEnemyBehaviour
    {
        //public float AttackTime { get; }
        //private GameObject GameObject { get; }
        public float MovementSpeed { get; } = 2f;
        public readonly float LaunchForce = 30f;  //30f
        public float NextFire { get;  set; } = 3.0f;
        public bool isActive=true;

        public int ReservedArea { get; set; }

        //private List<IShellBehaviour> ShootedShells { get;  } = new List<IShellBehaviour>();
        private Transform FireTransform { get; set; }

        private void Awake()
        {
            FireTransform = /*GameObject.*/GetComponentsInChildren<Transform>()[2];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"PlayerShell")) return;
            Destroy(gameObject);
            EnemySpawner.SpawnPoints[ReservedArea].IsActive = false;
            isActive = false;
        }


        public void Move()
        {
            var rigidBody = /*GameObject.*/GetComponent<Rigidbody>();
            var movement = /*GameObject.*/transform.forward * MovementSpeed * Time.deltaTime;
            rigidBody.MovePosition(rigidBody.position + movement);
            
        }

        public void Attack()
        {
            // create shell
            if (Time.time > NextFire)
            {
                NextFire = Time.time + Random.Range(1f, 3f);
                var enemyTigerShellObject = Instantiate(Resources.Load("prefabs/EnemyTigerShell", typeof(GameObject)) as GameObject);
                var enemyTigerShellRigidBody = enemyTigerShellObject.GetComponent<Rigidbody>();
                if (!enemyTigerShellRigidBody) return;
                //var transform = enemyTigerShell.transform;
                var transformShell = enemyTigerShellRigidBody.transform;
                transformShell.rotation = FireTransform.rotation;
                transformShell.position = FireTransform.position;
                enemyTigerShellRigidBody.velocity= LaunchForce  * FireTransform.forward;
               /* IShellBehaviour enemyTigerShell =*/ enemyTigerShellObject.AddComponent<TigerShellCollision>();
                //ShootedShells.Add(enemyTigerShell);S
            }
            //move all active shells
            //foreach (var shell in ShootedShells)
            //{
               
            //    if (shell.IsActive())
            //    {
            //        var rigidBody = shell.GetGameObject().GetComponent<Rigidbody>();
            //        rigidBody.velocity = LaunchForce * Time.deltaTime * FireTransform.forward;
            //        //var movement = FireTransform.forward * LaunchForce * Time.deltaTime;
            //        //shell.MovePosition(shell.position + movement);
            //    }
            //}

        }

        public bool IsActive()
        {
            return isActive;
        }
    }
}
