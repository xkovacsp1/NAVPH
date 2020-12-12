using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class AmmoBox : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool IsAlive { get; private set; } = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }
        public bool IsPowerUpActive { get; private set; }
        public float Timer { get; set; }
        public float powerUpDuration = 15.0f;
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
        private float IncreasedDamage { get; set; }
        public PowerUpsSpawner Spawner { get; set; }
        public float powerUpEffect = 0.25f;

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
                if (other.GetComponent<Player.Player>().ActivePowerUp)
                {
                    IsAlive = false;
                    Destroy(gameObject);
                    return;
                }

                var player = other.GetComponent<Player.Player>();
                IncreasedDamage = player.damage * powerUpEffect;
                player.damage += IncreasedDamage;
                player.abilityScoreHeader.SetActive(true);
                player.abilityTimeLeft.SetActive(true);
                player.abilityScoreHeaderText.text = "Increased damage";
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
                Player.GetComponent<Player.Player>().abilityTimeLeftText.text =
                    Math.Round((powerUpDuration - Timer)).ToString(CultureInfo.CurrentCulture);
                Timer += Time.deltaTime;

                if (Timer > powerUpDuration)
                {
                    var player = Player.GetComponent<Player.Player>();
                    player.abilityTimeLeftText.text =
                        Math.Round((powerUpDuration - Timer)).ToString(CultureInfo.CurrentCulture);
                    IsPowerUpActive = false;
                    Destroy(gameObject);
                    Spawner.spawnAreas[ReservedArea].isActive = false;
                    IsAlive = false;
                    player.abilityScoreHeader.SetActive(false);
                    player.abilityTimeLeft.SetActive(false);
                    player.damage -= IncreasedDamage;
                    player.ActivePowerUp = false;
                }
            }
        }
    }
}