using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.Scripts.Arena
{
    public class Arena : MonoBehaviour
    {
        public float timeForArena = 60f;
        public Text timeLeftText;
        public Player.Player Player { get; set; }
        public GameObject coinPrefab;
        public int coinNumber = 15;
        public Random Random { get; } = new Random();
        public Renderer Plane { get; private set; }
        public List<SpawnPoint> coinSpawnAreas = new List<SpawnPoint>()
        {
            {new SpawnPoint(14.39f,false)},
            { new SpawnPoint(9.27f, false)},
            { new SpawnPoint(4.5f,false)},
            {new SpawnPoint(-0.45f, false)},
            {new SpawnPoint(-5.53f, false)},
            { new SpawnPoint(-10.32f, false)}
        };
       

        private void Awake()
        {
            Plane = GameObject.FindWithTag("Plane").GetComponent<Renderer>();
            Player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
            timeLeftText.text = Math.Round(timeForArena).ToString(CultureInfo.CurrentCulture);
        }



        private void Start()
        {
            if(coinSpawnAreas.Count > 0)
                StartCoroutine(GenerateCoins());
        }


        private IEnumerator  GenerateCoins()
        {
            var coinCount = 0;
            while (coinCount < coinNumber)
            {
                
                var index = Random.Next(coinSpawnAreas.Count);
                var spawnPoint = coinSpawnAreas[index];
                var bounds = Plane.bounds;
                spawnPoint.ZPos = UnityEngine.Random.Range((-bounds.extents.x) + 5f, bounds.extents.z - 5f);
                if (!spawnPoint.isActive)
                {
                    if (GenerateCoin(spawnPoint))
                    {
                        coinSpawnAreas[index].isActive = true;
                        coinCount++;
                    }
                }
                yield return new WaitForSeconds(2f);
            }

           


        }

        public bool GenerateCoin(SpawnPoint spawnPoint)
        {
            if (coinPrefab == null)
            {
                return false;
            }

            var coin = Instantiate(coinPrefab);
            coin.transform.rotation = gameObject.transform.rotation;
            coin.transform.position = new Vector3(spawnPoint.xPos, 0, spawnPoint.ZPos);
            return true;
        }

        public void Update()
        {
            timeForArena -= Time.deltaTime;
            timeLeftText.text = Math.Round(timeForArena).ToString(CultureInfo.CurrentCulture);

            if (timeForArena <= 0f)
            {
                PlayerPrefs.SetInt("CollectedCoins", Player.NumberOfCollectedCoins);
                SceneManager.LoadScene(sceneBuildIndex: 3);
            }
        }


      

    }
}
