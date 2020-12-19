using System.Collections.Generic;
using Assets.Scripts.Shared;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {
        public List<SpawnPoint> spawnAreas = new List<SpawnPoint>()
        {
            {new SpawnPoint(14.39f, true)},
            {new SpawnPoint(9.27f, true)},
            {new SpawnPoint(4.5f, true)},
            {new SpawnPoint(-0.45f, true)},
            {new SpawnPoint(-5.53f, true)},
            {new SpawnPoint(-10.32f, true)}
        };

        public float spawnAreasOffset = 3f;
        public GameObject barrierPrefab;
        public GameObject rockPrefab;
        public GameObject minePrefab;


        public float YPos { get; } = 0;
        public Renderer Plane { get; private set; }

        public enum ObstacleTypes
        {
            Barrel,
            Rock
        }

        private void Awake()
        {
            Plane = GameObject.FindWithTag("Plane").GetComponent<Renderer>();
        }


        public void Start()
        {
            if (spawnAreas.Count > 0 && Plane)
                MakeObstacles();
        }

        private void MakeObstacles()
        {
            foreach (var spawnPoint in spawnAreas)
            {
                ObstacleTypes randomObstacleType = (ObstacleTypes) Random.Range(0, 3);
                var bounds = Plane.bounds;
                spawnPoint.ZPos = Random.Range((-bounds.extents.x) + spawnAreasOffset,
                    bounds.extents.x - spawnAreasOffset);
                switch (randomObstacleType)
                {
                    //generate Barrel
                    case ObstacleTypes.Barrel:
                        GenerateBarrier(spawnPoint);
                        break;

                    //generate Rock
                    case ObstacleTypes.Rock:
                        GenerateRock(spawnPoint);
                        break;
                    //generate Mine
                    default:
                        GenerateMine(spawnPoint);
                        break;
                }
            }
        }

        private void GenerateBarrier(SpawnPoint spawnPoint)
        {
            if (!barrierPrefab)
            {
                return;
            }

            Instantiate(barrierPrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos),
                gameObject.transform.rotation);
        }

        private void GenerateRock(SpawnPoint spawnPoint)
        {
            if (!rockPrefab)
            {
                return;
            }

            Instantiate(rockPrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos), gameObject.transform.rotation);
        }

        private void GenerateMine(SpawnPoint spawnPoint)
        {
            if (!minePrefab)
            {
                return;
            }

            Instantiate(minePrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos), gameObject.transform.rotation);
        }
    }
}