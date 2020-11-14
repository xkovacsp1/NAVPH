using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<EnemyBehaviourContext> EnemyBehaviors { get; } = new List<EnemyBehaviourContext>();
        public float zPos=20.18f;
        public float yPos;
        public float maxXPos = 14.87f;
        public float minXPos = -15.16f;
        public int enemyNumber = 1;


        public void Start()
        {
            StartCoroutine(MakeEnemies());
        }

        private IEnumerator MakeEnemies()
        {
            for (var i = 0; i < enemyNumber; i++)
            {
                //var enemyTiger = Resources.Load("prefabs/EnemyTiger", typeof(GameObject)) as GameObject;
                var enemyTiger = Instantiate(Resources.Load("prefabs/EnemyTiger", typeof(GameObject)) as GameObject);
                if (enemyTiger)
                {
                    enemyTiger.transform.rotation = gameObject.transform.rotation;
                    enemyTiger.transform.position = new Vector3(Random.Range(minXPos, maxXPos), yPos, zPos);
                    IEnemyBehaviour enemyBehavior = new TigerEnemy(enemyTiger);
                    EnemyBehaviourContext context = new EnemyBehaviourContext(enemyBehavior);
                    EnemyBehaviors.Add(context);
                }

                yield return new WaitForSeconds(1f);

                //if (i < 5)
                //{
                //    enemy.name = "Drone";
                //    enemyBehavior = new DroneBehavior(Time.time, enemy, minX, maxX);
                //    enemy.GetComponent<MeshRenderer>().material.color = Color.blue;
                //}
                //else
                //{
                //    enemy.name = "Fighter";
                //    enemyBehavior = new FighterBehavior(Time.time, enemy, minX, maxX);
                //    enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                //}
            }
        }


        public void Update()
        {
            foreach (var enemy in EnemyBehaviors)
            {
                enemy.Act();
            }
        }
    }
}
