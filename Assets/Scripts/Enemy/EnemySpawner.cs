using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy.Strategy;
using Assets.Scripts.Shared;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public List<EnemyBehaviourContext> EnemyBehaviors { get; } = new List<EnemyBehaviourContext>();

        public List<SpawnPoint> spawnAreas = new List<SpawnPoint>()
        {
            {new SpawnPoint(14.39f, false)},
            {new SpawnPoint(9.27f, false)},
            {new SpawnPoint(4.5f, false)},
            {new SpawnPoint(-0.45f, false)},
            {new SpawnPoint(-5.53f, false)},
            {new SpawnPoint(-10.32f, false)}
        };

        public float zPos = 45f;
        public float YPos { get; set; } = 0f;
        public int enemyNumber = 10;
        public Random Random { get; } = new Random();

        public GameObject enemyTigerPrefab;
        public GameObject enemySoldierPrefab;
        public GameObject enemyTruckPrefab;

        public enum EnemyTypes
        {
            EnemyTiger,
            EnemySoldier,
            EnemyTruck
        }

        public void Start()
        {
            if (spawnAreas.Count > 0)
                StartCoroutine(MakeEnemies());
        }

        private IEnumerator MakeEnemies()
        {
            var enemyCount = 0;
            while (enemyCount < enemyNumber)
            {
                var index = Random.Next(spawnAreas.Count);
                var spawnPoint = spawnAreas[index];
                if (!spawnPoint.isActive)
                {
                    EnemyTypes randomEnemyType = (EnemyTypes) Random.Next(0, 2);
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

                    spawnAreas[index].isActive = true;
                    EnemyBehaviors.Add(context);
                    enemyCount++;
                }

                yield return new WaitForSeconds(2f);
            }
        }

        private IEnemyBehaviour GenerateEnemyTiger(SpawnPoint spawnPoint, int index)
        {
            if (!enemyTigerPrefab)
            {
                return null;
            }

            var enemyTiger = Instantiate(enemyTigerPrefab);
            enemyTiger.transform.rotation = gameObject.transform.rotation;
            enemyTiger.transform.position = new Vector3(spawnPoint.xPos, YPos, zPos);
            enemyTiger.GetComponent<EnemyTiger>().ReservedArea = index;
            return enemyTiger.GetComponent<EnemyTiger>();
        }

        private IEnemyBehaviour GenerateEnemySoldier(SpawnPoint spawnPoint, int index)
        {
            if (!enemySoldierPrefab)
            {
                return null;
            }


            var enemySoldier = Instantiate(enemySoldierPrefab);
            enemySoldier.transform.rotation = gameObject.transform.rotation;
            enemySoldier.transform.position = new Vector3(spawnPoint.xPos, YPos, zPos);
            enemySoldier.AddComponent<EnemySoldier>();
            enemySoldier.GetComponent<EnemySoldier>().ReservedArea = index;
            return enemySoldier.GetComponent<EnemySoldier>();
        }

        //public IEnemyBehaviour GenerateEnemyTruck(EnemySpawnPoint spawnPoint, int index)
        //{
        //    var enemySoldier = Instantiate(Resources.Load(enemyTruckPrefab);
        //    enemySoldier.transform.rotation = gameObject.transform.rotation;
        //    enemySoldier.transform.position = new Vector3(spawnPoint.xPos, yPos, zPos);
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