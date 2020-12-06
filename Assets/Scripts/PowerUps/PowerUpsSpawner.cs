using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PowerUps.Strategy;
using UnityEngine;
using Random = System.Random;

    namespace Assets.Scripts.PowerUps
    {
        public class PowerUpsSpawner : MonoBehaviour
        {
            public List<PowerUpsBehaviourContext> PowerUpsBehaviors { get; } = new List<PowerUpsBehaviourContext>();
            public List<PowerUpSpawnPoint> SpawnPoints { get; set; } = new List<PowerUpSpawnPoint>()
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
            public int powerUpsNumber = 15;
            public Random Random { get; } = new Random();

            public GameObject ammoBoxPrefab;
            public GameObject barrelPrefab;
            public GameObject coinPrefab;
            public GameObject drillPrefab;

        public enum PowerUpTypes
            {
                Coin,Drill,AmmoBox
            }

            public void Start()
            {
               StartCoroutine(MakeEnemies());
            }

            private IEnumerator MakeEnemies()
            {
                var powerUpCount = 0;
                while (powerUpCount < powerUpsNumber)
                {
                    var index = Random.Next(SpawnPoints.Count);
                    var spawnPoint = SpawnPoints[index];
                    PowerUpsBehaviourContext context;
                    if (!spawnPoint.IsActive)
                    {
                        PowerUpTypes randomEnemyType =(PowerUpTypes) Random.Next(0,4);
                        switch (randomEnemyType)
                    {
                        //generate Coin
                        case PowerUpTypes.Coin:
                            context = new PowerUpsBehaviourContext(GenerateCoin(spawnPoint));
                            break;
                        //generate soldier
                        case PowerUpTypes.Drill:
                            context = new PowerUpsBehaviourContext(GenerateDrill(spawnPoint));
                            break;
                        case PowerUpTypes.AmmoBox:
                            context = new PowerUpsBehaviourContext(GenerateAmmoBox(spawnPoint));
                            break;
                        default:
                            context = new PowerUpsBehaviourContext(GenerateBarrel(spawnPoint));
                            break;
                        //generate enemy truck
                        //default:
                        //    context = new EnemyBehaviourContext(GenerateEnemyTruck(spawnPoint));
                        //    break;
                    }
                    //var context = new PowerUpsBehaviourContext(GenerateCoin(spawnPoint));
                    SpawnPoints[index].IsActive = true;
                    PowerUpsBehaviors.Add(context);
                    powerUpCount++;
                    }
                    yield return new WaitForSeconds(2f);
                }
            }


        public IPowerUpsBehaviour GenerateCoin(PowerUpSpawnPoint spawnPoint)
        {
            var coin = Instantiate(coinPrefab);
            coin.transform.rotation = gameObject.transform.rotation;
            coin.transform.position = new Vector3(spawnPoint.XPos, yPos, spawnPoint.ZPos);
            return coin.AddComponent<Coin>();
        }

        public IPowerUpsBehaviour GenerateDrill(PowerUpSpawnPoint spawnPoint)
        {
            var drill = Instantiate(drillPrefab);
            drill.transform.rotation = gameObject.transform.rotation;
                                                                     // increase y psorion of drill
            drill.transform.position = new Vector3(spawnPoint.XPos, yPos+0.5f, spawnPoint.ZPos);
            return drill.AddComponent<Drill>();
        }

        public IPowerUpsBehaviour GenerateAmmoBox(PowerUpSpawnPoint spawnPoint)
        {
            var ammoBox = Instantiate(ammoBoxPrefab);
            ammoBox.transform.rotation = gameObject.transform.rotation;
            ammoBox.transform.position = new Vector3(spawnPoint.XPos, yPos, spawnPoint.ZPos);
            return ammoBox.AddComponent<AmmoBox>();
        }

        public IPowerUpsBehaviour GenerateBarrel(PowerUpSpawnPoint spawnPoint)
        {
            var barrel = Instantiate(barrelPrefab);
            barrel.transform.rotation = gameObject.transform.rotation;
            barrel.transform.position = new Vector3(spawnPoint.XPos, yPos, spawnPoint.ZPos);
            return barrel.AddComponent<Barrel>();
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
