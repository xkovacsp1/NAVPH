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
        public float PowerUpDuration { get; } = 5.0f;
        public GameObject Player { get; set; }
        public Rigidbody RigidBody { get; set; }
    

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player");
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;

            other.GetComponent<Player.Player>().damage += other.GetComponent<Player.Player>().damage*0.25f;
            other.GetComponent<Player.Player>().abilityScoreHeader.SetActive(true);
            other.GetComponent<Player.Player>().abilityTimeLeft.SetActive(true);
            IsPowerUpActive = true;
            //Destroy(gameObject);      // nemozes odstranit lebo takeeffect neskor potrebuje
            gameObject.SetActive(false);
            //PowerUpsSpawner.SpawnPoints[ReservedArea].IsActive = false;
            //isActive = false;
            
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
                Player.GetComponent<Player.Player>().abilityTimeLeftText.text = Math.Round((5.0f - Timer)).ToString();
                Timer += Time.deltaTime;

                if (Timer > PowerUpDuration)
                {
                    Player.GetComponent<Player.Player>().abilityTimeLeftText.text = Math.Round((5.0f-Timer)).ToString();
                    IsPowerUpActive = false;
                    Destroy(gameObject);
                    PowerUpsSpawner.SpawnPoints[ReservedArea].IsActive = false;
                    isActive = false;
                    Player.GetComponent<Player.Player>().abilityScoreHeader.SetActive(false);
                    Player.GetComponent<Player.Player>().abilityTimeLeft.SetActive(false);
                    Player.GetComponent<Player.Player>().damage -=Player.GetComponent<Player.Player>().damage * 0.25f;
                }
            }


        }
    }
}
