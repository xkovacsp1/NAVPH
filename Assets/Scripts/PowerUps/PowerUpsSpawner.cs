using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;
using Random = System.Random;

    namespace Assets.Scripts.PowerUps
    {
        public class PowerUpsSpawner : MonoBehaviour
        {
            public List<PowerUpsBehaviourContext> PowerUpsBehaviors { get; } = new List<PowerUpsBehaviourContext>();
            public static List<PowerUpSpawnPoint> SpawnPoints { get; set; } = new List<PowerUpSpawnPoint>()
            {
                {new PowerUpSpawnPoint(14.39f,40.0f, false)},
                { new PowerUpSpawnPoint(9.27f,30.0f, false)},
                { new PowerUpSpawnPoint(4.5f, 20.0f,false)},
                {new PowerUpSpawnPoint(-0.45f,10.0f, false)},
                {new PowerUpSpawnPoint(-5.53f,0.0f, false)},
                { new PowerUpSpawnPoint(-10.32f,-10.0f, false)}
            };

            //public float zPos = 45f;
            public float yPos;
            //public float maxXPos = 14.87f;
            //public float minXPos = -15.16f;
            public int powerUpsNumber = 10;
            public Random Random { get; } = new Random();

            public enum PowerUpTypes
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
                while (enemyCount < powerUpsNumber)
                {
                    var index = Random.Next(SpawnPoints.Count);
                    var spawnPoint = SpawnPoints[index];
                    if (!spawnPoint.IsActive)
                    {
                        //PowerUpTypes randomEnemyType =(PowerUpTypes) Random.Next(0,2);
                        //switch (randomEnemyType)
                    //{
                    //    //generate tiger
                    //    case PowerUpTypes.EnemyTiger:
                    //        context = new EnemyBehaviourContext(GenerateEnemyTiger(spawnPoint));
                    //        break;
                    //    //generate soldier
                    //    //case EnemyTypes.EnemySoldier:
                    //    default:
                    //        context = new EnemyBehaviourContext(GenerateEnemySoldier(spawnPoint));
                    //        break;
                    //    //generate enemy truck
                    //    //default:
                    //    //    context = new EnemyBehaviourContext(GenerateEnemyTruck(spawnPoint));
                    //    //    break;
                    //}
                    var context = new PowerUpsBehaviourContext(GenerateCoin(spawnPoint));
                    SpawnPoints[index].IsActive = true;
                    PowerUpsBehaviors.Add(context);
                        enemyCount++;
                    }
                    yield return new WaitForSeconds(2f);
                }
            }


        public IPowerUpsBehaviour GenerateCoin(PowerUpSpawnPoint spawnPoint)
        {
            var coin = Instantiate(Resources.Load("prefabs/Coin", typeof(GameObject)) as GameObject);
            coin.transform.rotation = gameObject.transform.rotation;
            coin.transform.position = new Vector3(spawnPoint.XPos, yPos, spawnPoint.ZPos);
            return coin.AddComponent<Coin>();
        }

            public void Update()
            {
                foreach (var powerup in PowerUpsBehaviors)
                {
                    powerup.Act();
                }
            }
        }
    }
