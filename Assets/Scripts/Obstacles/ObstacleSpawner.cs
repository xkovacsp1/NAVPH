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

        public float spawnAreasOffset = 5f;
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
            if (spawnAreas.Count > 0)
                MakeObstacles();
        }

        private void MakeObstacles()
        {
            foreach (var spawnPoint in spawnAreas)
            {
                ObstacleTypes randomObstacleType = (ObstacleTypes) Random.Range(0, 3);
                var bounds = Plane.bounds;
                spawnPoint.ZPos = Random.Range((-bounds.extents.x) + spawnAreasOffset, bounds.extents.z - spawnAreasOffset);
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

            var barrier = Instantiate(barrierPrefab);
            barrier.transform.rotation = gameObject.transform.rotation;
            barrier.transform.position = new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos);
        }

        private void GenerateRock(SpawnPoint spawnPoint)
        {
            if (!rockPrefab)
            {
                return;
            }

            var rock = Instantiate(rockPrefab);
            rock.transform.rotation = gameObject.transform.rotation;
            rock.transform.position = new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos);
        }

        private void GenerateMine(SpawnPoint spawnPoint)
        {
            if (!minePrefab)
            {
                return;
            }

            var mine = Instantiate(minePrefab);
            mine.transform.rotation = gameObject.transform.rotation;
            mine.transform.position = new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos);
        }
    }
}