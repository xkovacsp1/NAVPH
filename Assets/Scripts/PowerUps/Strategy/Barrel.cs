﻿using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class Barrel : MonoBehaviour, IPowerUpsBehaviour
    {
        public float rotationSpeed = 60f;
        public float powerUpDuration = 10.0f;
        public float powerUpEffect = 0.25f;
        public AudioClip collisionSound;

        public bool IsAlive { get; private set; } = true;
        public int ReservedArea { get; set; }
        public bool IsPowerUpActive { get; private set; }
        public float Timer { get; set; }
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
        private float IncreasedSpeed { get; set; }
        public PowerUpsSpawner Spawner { get; set; }
        public GamePlay GamePlayCanvas { get; set; }
        public string PowerUpType { get; set; } = "Increased Speed";

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            Player = GameObject.FindWithTag("Player");
            RigidBody = GetComponent<Rigidbody>();
            GamePlayCanvas = GameObject.FindWithTag("GamePlayCanvas").GetComponent<GamePlay>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player") && !other.GetComponent<Player.Player>().ActivePowerUp)
            {
                if (collisionSound && RigidBody)
                {
                    AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
                }

                var player = other.GetComponent<Player.Player>();
                if (!player) return;

                IncreasedSpeed = player.speed * powerUpEffect;
                player.speed += IncreasedSpeed;
                IsPowerUpActive = true;
                player.ActivePowerUp = true;
                gameObject.SetActive(false);
            }
        }

        public void Move()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        public bool IsActive()
        {
            return IsAlive;
        }

        /* this function is called every frame and checks the actual state of powerup
           if time for powerup expires then powerup effect is disabled */
        public void TakeEffect()
        {
            if (IsPowerUpActive)
            {
                if (GamePlayCanvas)
                    GamePlayCanvas.ShowPowerUpLefTime(PowerUpType, powerUpDuration - Timer);
                Timer += Time.deltaTime;

                if (Timer > powerUpDuration)
                {
                    var player = Player.GetComponent<Player.Player>();
                    if (!player) return;
                    IsPowerUpActive = false;
                    if (Spawner)
                        Spawner.spawnAreas[ReservedArea].isActive = false;
                    IsAlive = false;
                    if (GamePlayCanvas)
                        GamePlayCanvas.HidePowerUpLefTime(powerUpDuration - Timer);
                    player.speed -= IncreasedSpeed;
                    player.ActivePowerUp = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}