using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<EnemyBehaviourContext> EnemyBehaviors { get; } = new List<EnemyBehaviourContext>();

        public static List<EnemySpawnPoint> SpawnPoints { get; set; } = new List<EnemySpawnPoint>()
        {
            {new EnemySpawnPoint(14.39f, false)},
            { new EnemySpawnPoint(9.27f, false)},
            { new EnemySpawnPoint(4.5f, false)},
            {new EnemySpawnPoint(-0.45f, false)},
            {new EnemySpawnPoint(-5.53f, false)},
            { new EnemySpawnPoint(-10.32f, false)}
        };

        public float zPos = 20.18f;
        public float yPos;
        //public float maxXPos = 14.87f;
        //public float minXPos = -15.16f;
        public int enemyNumber = 10;
        public Random Random { get; } = new Random();

        public void Start()
        {
            StartCoroutine(MakeEnemies());
        }

        private IEnumerator MakeEnemies()
        {
            var enemyCount = 0;
            while (enemyCount < enemyNumber)
            {
                var index = Random.Next(SpawnPoints.Count);
                var spawnPoint = SpawnPoints[index];
                if (!spawnPoint.IsActive)
                {
                    var enemyTiger = Instantiate(Resources.Load("prefabs/EnemyTiger", typeof(GameObject)) as GameObject);
                    enemyTiger.transform.rotation = gameObject.transform.rotation;
                    enemyTiger.transform.position = new Vector3(spawnPoint.XPos, yPos, zPos);
                    SpawnPoints[index].IsActive = true;
                    IEnemyBehaviour enemyBehavior = enemyTiger.AddComponent<TigerEnemy>(); 
                    // set the reserved area
                    enemyTiger.GetComponent<TigerEnemy>().ReservedArea = index;
                    EnemyBehaviourContext context = new EnemyBehaviourContext(enemyBehavior);
                    EnemyBehaviors.Add(context);
                    enemyCount++;
                }
                yield return new WaitForSeconds(3f);
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