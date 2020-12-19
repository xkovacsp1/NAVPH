using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PowerUps.Strategy;
using Assets.Scripts.Shared;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.PowerUps
{
    public class PowerUpsSpawner : MonoBehaviour
    {
        public List<SpawnPoint> spawnAreas = new List<SpawnPoint>()
        {
            {new SpawnPoint(14.39f, false)},
            {new SpawnPoint(9.27f, false)},
            {new SpawnPoint(4.5f, false)},
            {new SpawnPoint(-0.45f, false)},
            {new SpawnPoint(-5.53f, false)},
            {new SpawnPoint(-10.32f, false)}
        };

        public float spawnAreasOffset = 3f;
        public float spawnIntervalLength = 11f;
        public int powerUpsNumber = 10;
        public GameObject ammoBoxPrefab;
        public GameObject barrelPrefab;
        public GameObject drillPrefab;

        public List<PowerUpsBehaviourContext> PowerUpsBehaviors { get; } = new List<PowerUpsBehaviourContext>();
        public float YPos { get; } = 0;
        public Random Random { get; } = new Random();
        public Renderer Plane { get; private set; }

        private void Awake()
        {
            Plane = GameObject.FindWithTag("Plane").GetComponent<Renderer>();
        }


        public enum PowerUpTypes
        {
            Drill,
            AmmoBox
        }

        public void Start()
        {
            if (spawnAreas.Count > 0 && Plane)
                StartCoroutine(MakePowerUps());
        }

        private IEnumerator MakePowerUps()
        {
            var powerUpCount = 0;
            while (powerUpCount < powerUpsNumber)
            {
                var index = Random.Next(spawnAreas.Count);
                var spawnPoint = spawnAreas[index];
                var bounds = Plane.bounds;
                spawnPoint.ZPos = UnityEngine.Random.Range((-bounds.extents.x) + spawnAreasOffset,
                    bounds.extents.x - spawnAreasOffset);
                if (!spawnPoint.isActive)
                {
                    PowerUpTypes randomEnemyType = (PowerUpTypes) Random.Next(0, 3);
                    PowerUpsBehaviourContext context;
                    switch (randomEnemyType)
                    {
                        case PowerUpTypes.Drill:
                            context = new PowerUpsBehaviourContext(GenerateDrill(spawnPoint));
                            break;
                        case PowerUpTypes.AmmoBox:
                            context = new PowerUpsBehaviourContext(GenerateAmmoBox(spawnPoint));
                            break;
                        default:
                            context = new PowerUpsBehaviourContext(GenerateBarrel(spawnPoint));
                            break;
                    }

                    spawnAreas[index].isActive = true;
                    PowerUpsBehaviors.Add(context);
                    powerUpCount++;
                }

                yield return new WaitForSeconds(spawnIntervalLength);
            }
        }

        private IPowerUpsBehaviour GenerateDrill(SpawnPoint spawnPoint)
        {
            if (!drillPrefab)
            {
                return null;
            }

            var drill = Instantiate(drillPrefab, new Vector3(spawnPoint.xPos, YPos + 0.5f, spawnPoint.ZPos),
                gameObject.transform.rotation);
            return drill.GetComponent<Drill>();
        }

        private IPowerUpsBehaviour GenerateAmmoBox(SpawnPoint spawnPoint)
        {
            if (!ammoBoxPrefab)
            {
                return null;
            }

            var ammoBox = Instantiate(ammoBoxPrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos),
                gameObject.transform.rotation);
            return ammoBox.GetComponent<AmmoBox>();
        }

        private IPowerUpsBehaviour GenerateBarrel(SpawnPoint spawnPoint)
        {
            if (!barrelPrefab)
            {
                return null;
            }

            var barrel = Instantiate(barrelPrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos),
                gameObject.transform.rotation);
            return barrel.GetComponent<Barrel>();
        }

        public void FixedUpdate()
        {
            foreach (var powerUp in PowerUpsBehaviors)
            {
                if (powerUp.PowerUpBehavior == null)
                {
                    continue;
                }

                powerUp.Act();
            }
        }
    }
}