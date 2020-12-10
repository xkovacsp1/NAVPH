using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PowerUps;
using Assets.Scripts.PowerUps.Strategy;
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
        public List<PowerUpSpawnPoint> SpawnPoints { get; set; } = new List<PowerUpSpawnPoint>()
        {
            {new PowerUpSpawnPoint(14.39f,45.0f, false)},
            { new PowerUpSpawnPoint(9.27f,35.0f, false)},
            { new PowerUpSpawnPoint(4.5f, 25.0f,false)},
            {new PowerUpSpawnPoint(-0.45f,15.0f, false)},
            {new PowerUpSpawnPoint(-5.53f,0.0f, false)},
            { new PowerUpSpawnPoint(-10.32f,-15.0f, false)}
        };



        private void Start()
        {
            StartCoroutine(GenerateCoins());
        }


        private IEnumerator  GenerateCoins()
        {
            var coinCount = 0;
            while (coinCount < coinNumber)
            {
                
                var index = Random.Next(SpawnPoints.Count);
                var spawnPoint = SpawnPoints[index];
                if (!spawnPoint.IsActive)
                {
                    if (GenerateCoin(spawnPoint))
                    {
                        SpawnPoints[index].IsActive = true;
                        coinCount++;
                    }
                }
                yield return new WaitForSeconds(2f);
            }

           


        }

        public bool GenerateCoin(PowerUpSpawnPoint spawnPoint)
        {
            if (coinPrefab == null)
            {
                return false;
            }

            var coin = Instantiate(coinPrefab);
            coin.transform.rotation = gameObject.transform.rotation;
            coin.transform.position = new Vector3(spawnPoint.XPos, 0, spawnPoint.ZPos);
            coin.AddComponent<Coin>();
            return true;
        }

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
            timeLeftText.text = Math.Round(timeForArena).ToString();
           
        }

        public void Update()
        {
            timeForArena -= Time.deltaTime;
            timeLeftText.text = Math.Round(timeForArena).ToString();

            if (timeForArena <= 0f)
            {
                PlayerPrefs.SetInt("CollectedCoins", Player.numberOfCollectedCoins);
                SceneManager.LoadScene(sceneBuildIndex: 3);
            }
        }


      

    }
}
