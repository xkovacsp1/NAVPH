using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
namespace Assets.Scripts.Obstacles
{
    public class ObstacleSpawner : MonoBehaviour
    {

        public List<ObstacleSpawnPoint> SpawnPoints { get; set; } = new List<ObstacleSpawnPoint>()
        {
            {new ObstacleSpawnPoint(14.39f, 35.0f)},
            {new ObstacleSpawnPoint(9.27f, 25.0f)},
            {new ObstacleSpawnPoint(4.5f, 15.0f)},
            {new ObstacleSpawnPoint(-0.45f, 5.0f)},
            {new ObstacleSpawnPoint(-5.53f, -5.0f)},
            {new ObstacleSpawnPoint(-10.32f, -15.0f)}
        };

        //public float zPos = 45f;
        public float YPos { get; } = 0;

        //public float maxXPos = 14.87f;
        //public float minXPos = -15.16f;
        //public int obstacleNumber = 15;
        public Random Random { get; } = new Random();
        public GameObject barrierPrefab;
        public GameObject rockPrefab;
        public GameObject minePrefab;
        public enum ObstacleTypes
        {
            Barrel,Rock
        }

        public void Start()
        {
            MakeObstacles();
            //StartCoroutine(MakeEnemies());
        }

        //private IEnumerator MakeEnemies()
        private void MakeObstacles()
        {
            foreach (var spawPoint in SpawnPoints)
            {
                ObstacleTypes randomObstacleType = (ObstacleTypes)Random.Next(0, 3);
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
            var barrier = Instantiate(barrierPrefab);
            barrier.transform.rotation = gameObject.transform.rotation;
            barrier.transform.position = new Vector3(spawnPoint.XPos, YPos, spawnPoint.ZPos);
        }

        void GenerateRock(ObstacleSpawnPoint spawnPoint)
        {
            var rock = Instantiate(rockPrefab);
            rock.transform.rotation = gameObject.transform.rotation;
            rock.transform.position = new Vector3(spawnPoint.XPos, YPos, spawnPoint.ZPos);
        }

        void GenerateMine(ObstacleSpawnPoint spawnPoint)
        {
            var mine = Instantiate(minePrefab);
            mine.transform.rotation = gameObject.transform.rotation;
            mine.transform.position = new Vector3(spawnPoint.XPos, YPos, spawnPoint.ZPos);
        }

    }

}



