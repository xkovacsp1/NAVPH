using System;
using UnityEngine;
namespace Assets.Scripts.PowerUps.Strategy
{
    public class Barrel : MonoBehaviour, IPowerUpsBehaviour
    {
        public bool isActive = true;
        public float rotationSpeed = 60f;
        public int ReservedArea { get; set; }
        public bool IsPowerUpActive { get; private set; }
        public float Timer { get; set; }
        public float PowerUpDuration { get; } = 15.0f;
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
        private float IncreasedSpeed { get; set; }


        private void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                if (other.GetComponent<Player.Player>().ActivePowerUp)
                {
                    isActive = false;
                    Destroy(gameObject);
                    return;
                }

                var player = other.GetComponent<Player.Player>();
                IncreasedSpeed = player.speed * 0.25f;
                player.speed += IncreasedSpeed;
                player.abilityScoreHeader.SetActive(true);
                player.abilityTimeLeft.SetActive(true);
                player.abilityScoreHeaderText.text = "Increased speed";
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
                    PowerUpsSpawner.SpawnPoints[ReservedArea].IsActive = false;
                    isActive = false;
                    player.abilityScoreHeader.SetActive(false);
                    player.abilityTimeLeft.SetActive(false);
                    player.speed -= IncreasedSpeed;
                    player.ActivePowerUp = false;
                }
            }


        }
    }
}
