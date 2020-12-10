using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy.Strategy;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<EnemyBehaviourContext> EnemyBehaviors { get; } = new List<EnemyBehaviourContext>();
        public List<EnemySpawnPoint> SpawnPoints { get; set; } = new List<EnemySpawnPoint>()
        {
            {new EnemySpawnPoint(14.39f, false)},
            { new EnemySpawnPoint(9.27f, false)},
            { new EnemySpawnPoint(4.5f, false)},
            {new EnemySpawnPoint(-0.45f, false)},
            {new EnemySpawnPoint(-5.53f, false)},
            { new EnemySpawnPoint(-10.32f, false)}
        };

        public float zPos = 45f;
        public float yPos;
        //public float maxXPos = 14.87f;
        //public float minXPos = -15.16f;
        public int enemyNumber = 10;
        public Random Random { get; } = new Random();

        public GameObject enemyTigerPrefab;
        public GameObject enemySoldierPrefab;
        public GameObject enemyTruckPrefab;

        public enum EnemyTypes
        {
            EnemyTiger,EnemySoldier,EnemyTruck
        }

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
                    EnemyTypes randomEnemyType =(EnemyTypes) Random.Next(0,2);
                    EnemyBehaviourContext context;
                    switch (randomEnemyType)
                    {
                        //generate tiger
                        case EnemyTypes.EnemyTiger:

                            context = new EnemyBehaviourContext(GenerateEnemyTiger(spawnPoint, index));
                            break;
                        //generate soldier
                        //case EnemyTypes.EnemySoldier:
                        default:
                            context = new EnemyBehaviourContext(GenerateEnemySoldier(spawnPoint, index));
                            break;
                        //generate enemy truck
                        //default:
                        //    context = new EnemyBehaviourContext(GenerateEnemyTruck(spawnPoint));
                        //    break;
                    }
                    SpawnPoints[index].IsActive = true;
                    EnemyBehaviors.Add(context);
                    enemyCount++;
                }
                yield return new WaitForSeconds(2f);
            }
        }

        public IEnemyBehaviour GenerateEnemyTiger(EnemySpawnPoint spawnPoint, int index)
        {
            if (!enemyTigerPrefab)
            {
                return null;
            }

            var enemyTiger = Instantiate(enemyTigerPrefab);
            enemyTiger.transform.rotation = gameObject.transform.rotation;
            enemyTiger.transform.position = new Vector3(spawnPoint.XPos, yPos, zPos);
            //enemyTiger.AddComponent<EnemyTiger>();
            enemyTiger.GetComponent<EnemyTiger>().ReservedArea = index;
            return enemyTiger.GetComponent<EnemyTiger>();
        }

        public IEnemyBehaviour GenerateEnemySoldier(EnemySpawnPoint spawnPoint, int index)
        {
            if (!enemySoldierPrefab)
            {
                return null;
            }


            var enemySoldier = Instantiate(enemySoldierPrefab);
            enemySoldier.transform.rotation = gameObject.transform.rotation;
            enemySoldier.transform.position = new Vector3(spawnPoint.XPos, yPos, zPos);
            enemySoldier.AddComponent<EnemySoldier>();
            enemySoldier.GetComponent<EnemySoldier>().ReservedArea = index;
            return enemySoldier.GetComponent<EnemySoldier>();
        }

        //public IEnemyBehaviour GenerateEnemyTruck(EnemySpawnPoint spawnPoint, int index)
        //{
        //    var enemySoldier = Instantiate(Resources.Load(enemyTruckPrefab);
        //    enemySoldier.transform.rotation = gameObject.transform.rotation;
        //    enemySoldier.transform.position = new Vector3(spawnPoint.XPos, yPos, zPos);
        //    return enemySoldier.AddComponent<EnemyTruck>();
        //}
        public void FixedUpdate()
        {
            foreach (var enemy in EnemyBehaviors)
            {
                if (enemy.EnemyBehavior == null)
                {
                    continue;
                }
                enemy.Act();
            }
        }
    }
}