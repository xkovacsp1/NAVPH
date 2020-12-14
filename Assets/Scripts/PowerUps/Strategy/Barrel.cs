using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class Barrel : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool IsAlive { get; private set; } = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }
        public bool IsPowerUpActive { get; private set; }
        public float Timer { get; set; }
        public float powerUpDuration = 15.0f;
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
        private float IncreasedSpeed { get; set; }
        public PowerUpsSpawner Spawner { get; set; }
        public float powerUpEffect = 0.25f;
        public AudioClip collisionSound;


        private void Awake()
        {
            Spawner = GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            Player = GameObject.FindWithTag("Player");
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player") && !other.GetComponent<Player.Player>().ActivePowerUp)
            {
             
                if (collisionSound)
                {
                    AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
                }
                var player = other.GetComponent<Player.Player>();
                if (!player) return;

                IncreasedSpeed = player.speed * powerUpEffect;
                player.speed += IncreasedSpeed;
                player.abilityScoreHeader.SetActive(true);
                player.abilityTimeLeft.SetActive(true);
                player.abilityScoreHeaderText.text = "Increased speed";
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

        public void TakeEffect()
        {
            if (IsPowerUpActive)
            {
                if (!Player.GetComponent<Player.Player>()) return;
                Player.GetComponent<Player.Player>().abilityTimeLeftText.text =
                    Math.Round((powerUpDuration - Timer)).ToString(CultureInfo.CurrentCulture);
                Timer += Time.deltaTime;

                if (Timer > powerUpDuration)
                {
                    var player = Player.GetComponent<Player.Player>();
                    if (!player) return;
                    player.abilityTimeLeftText.text =
                        Math.Round((powerUpDuration - Timer)).ToString(CultureInfo.CurrentCulture);
                    IsPowerUpActive = false;
                    Destroy(gameObject);
                    Spawner.spawnAreas[ReservedArea].isActive = false;
                    IsAlive = false;
                    player.abilityScoreHeader.SetActive(false);
                    player.abilityTimeLeft.SetActive(false);
                    player.speed -= IncreasedSpeed;
                    player.ActivePowerUp = false;
                }
            }
        }
    }
}