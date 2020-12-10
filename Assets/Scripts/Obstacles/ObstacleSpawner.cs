using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {

        public List<ObstacleSpawnPoint> SpawnPoints { get; set; } = new List<ObstacleSpawnPoint>()
        {
            //{new ObstacleSpawnPoint(14.39f, 35.0f)},
            //{new ObstacleSpawnPoint(9.27f, 25.0f)},
            //{new ObstacleSpawnPoint(4.5f, 15.0f)},
            //{new ObstacleSpawnPoint(-0.45f, 5.0f)},
            //{new ObstacleSpawnPoint(-5.53f, -5.0f)},
            //{new ObstacleSpawnPoint(-10.32f, -15.0f)}

            {new ObstacleSpawnPoint(14.39f)},
            {new ObstacleSpawnPoint(9.27f)},
            {new ObstacleSpawnPoint(4.5f)},
            {new ObstacleSpawnPoint(-0.45f)},
            {new ObstacleSpawnPoint(-5.53f)},
            {new ObstacleSpawnPoint(-10.32f)}
        };


        public float YPos { get; } = 0;
        public Renderer Plane { get; private set; }

        public GameObject barrierPrefab;
        public GameObject rockPrefab;
        public GameObject minePrefab;
        public enum ObstacleTypes
        {
            Barrel,Rock
        }

        private void Awake()
        {
            Plane = GameObject.FindWithTag("Plane").GetComponent<Renderer>();
        }


        public void Start()
        {
            MakeObstacles();
          
        }

        private void MakeObstacles()
        {
            foreach (var spawPoint in SpawnPoints)
            {
                ObstacleTypes randomObstacleType = (ObstacleTypes)Random.Range(0, 3);
                var bounds = Plane.bounds;
                spawPoint.ZPos = Random.Range((-bounds.extents.x)+5f, bounds.extents.z - 5f);
                switch (randomObstacleType)
                {
                    //generate Barrel
                    case ObstacleTypes.Barrel:
                        GenerateBarrier(spawPoint);
                        break;

                    //generate Rock
                    case ObstacleTypes.Rock:
                        GenerateRock(spawPoint);
                        break;
                    //generate Mine
                    default:
                        GenerateMine(spawPoint);
                        break;

                }

            }

        }

        void GenerateBarrier(ObstacleSpawnPoint spawnPoint)
        {
            if (!barrierPrefab)
            {
                return;
            }

            var barrier = Instantiate(barrierPrefab);
            barrier.transform.rotation = gameObject.transform.rotation;
            barrier.transform.position = new Vector3(spawnPoint.XPos, YPos,spawnPoint.ZPos);
        }

        void GenerateRock(ObstacleSpawnPoint spawnPoint)
        {
            if (!rockPrefab)
            {
                return;
            }
            var rock = Instantiate(rockPrefab);
            rock.transform.rotation = gameObject.transform.rotation;
            rock.transform.position = new Vector3(spawnPoint.XPos, YPos, spawnPoint.ZPos);
        }

        void GenerateMine(ObstacleSpawnPoint spawnPoint)
        {
            if (!minePrefab)
            {
                return;
            }
            var mine = Instantiate(minePrefab);
            mine.transform.rotation = gameObject.transform.rotation;
            mine.transform.position = new Vector3(spawnPoint.XPos, YPos, spawnPoint.ZPos);
        }

    }

}



