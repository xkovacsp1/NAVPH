using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Assets.Scripts.Arena
{
    public class Arena : MonoBehaviour
    {
        public float timeForArena = 60f;
        public GameObject coinPrefab;
        public int coinNumber = 15;

        public List<SpawnPoint> coinSpawnAreas = new List<SpawnPoint>()
        {
            {new SpawnPoint(14.39f, false)},
            {new SpawnPoint(9.27f, false)},
            {new SpawnPoint(4.5f, false)},
            {new SpawnPoint(-0.45f, false)},
            {new SpawnPoint(-5.53f, false)},
            {new SpawnPoint(-10.32f, false)}
        };

        public float spawnAreasOffset = 3f;
        public float spawnIntervalLength = 10f;


        public Player.Player Player { get; set; }
        public Random Random { get; } = new Random();
        public Renderer Plane { get; private set; }
        public float YPos { get; set; } = 0f;


        private void Awake()
        {
            Plane = GameObject.FindWithTag("Plane").GetComponent<Renderer>();
            Player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
        }


        private void Start()
        {
            if (coinSpawnAreas.Count > 0 && Plane)
                StartCoroutine(GenerateCoins());
        }


        private IEnumerator GenerateCoins()
        {
            var coinCount = 0;
            while (coinCount < coinNumber)
            {
                var index = Random.Next(coinSpawnAreas.Count);
                var spawnPoint = coinSpawnAreas[index];
                var bounds = Plane.bounds;
                spawnPoint.ZPos = UnityEngine.Random.Range((-bounds.extents.x) + spawnAreasOffset,
                    bounds.extents.x - spawnAreasOffset);
                if (!spawnPoint.isActive)
                {
                    if (GenerateCoin(spawnPoint))
                    {
                        coinSpawnAreas[index].isActive = true;
                        coinCount++;
                    }
                }

                yield return new WaitForSeconds(spawnIntervalLength);
            }
        }

        public bool GenerateCoin(SpawnPoint spawnPoint)
        {
            if (coinPrefab == null)
            {
                return false;
            }

            Instantiate(coinPrefab, new Vector3(spawnPoint.xPos, YPos, spawnPoint.ZPos),
                gameObject.transform.rotation);
            return true;
        }

        public void Update()
        {
            timeForArena -= Time.deltaTime;

            if (timeForArena <= 0.0f)
            {
                if (Player)
                    PlayerPrefs.SetInt("CollectedCoins", Player.NumberOfCollectedCoins);
                SceneManager.LoadScene(sceneBuildIndex: 3);
            }
        }
    }
}