using System;
using UnityEngine;
namespace Assets.Scripts.PowerUps.Strategy
{
    public class AmmoBox : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool isActive = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }
        public bool IsPowerUpActive { get; private set; }
        public float Timer { get; set; }
        public float PowerUpDuration { get; } = 15.0f;
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
        private float IncreasedDamage { get; set; }
        public PowerUpsSpawner Spawner { get; set; }
        private void Awake()
        {
            Spawner= GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            Player = GameObject.FindWithTag("Player");
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player") && !other.GetComponent<Player.Player>().ActivePowerUp)
            {
                if (other.GetComponent<Player.Player>().ActivePowerUp)
                {
                    isActive = false;
                    Destroy(gameObject);
                    return;
                }

                var player = other.GetComponent<Player.Player>();
                IncreasedDamage = player.damage * 0.25f;
                player.damage += IncreasedDamage;
                player.abilityScoreHeader.SetActive(true);
                player.abilityTimeLeft.SetActive(true);
                player.abilityScoreHeaderText.text = "Increased damage";
                IsPowerUpActive = true;
                player.ActivePowerUp = true;
                //Destroy(gameObject);      // nemozes odstranit lebo takeeffect neskor potrebuje
                gameObject.SetActive(false);
                //PowerUpsSpawner.SpawnPoints[ReservedArea].IsActive = false;
                //isActive = false;
            }
        }

        public void Move()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void TakeEffect()
        {
            if (IsPowerUpActive)
            {
                Player.GetComponent<Player.Player>().abilityTimeLeftText.text = Math.Round((PowerUpDuration - Timer)).ToString();
                Timer += Time.deltaTime;

                if (Timer > PowerUpDuration)
                {
                    var player = Player.GetComponent<Player.Player>();
                    player.abilityTimeLeftText.text = Math.Round((PowerUpDuration - Timer)).ToString();
                    IsPowerUpActive = false;
                    Destroy(gameObject);
                    Spawner.SpawnPoints[ReservedArea].IsActive = false;
                    isActive = false;
                    player.abilityScoreHeader.SetActive(false);
                    player.abilityTimeLeft.SetActive(false);
                    player.damage -= IncreasedDamage;
                    player.ActivePowerUp = false;
                }
            }


        }
    }
}
