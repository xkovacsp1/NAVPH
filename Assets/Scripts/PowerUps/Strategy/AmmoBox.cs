using Assets.Scripts.UI;
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
        public GamePlay GamePlayCanvas { get; set; }
        public string PowerUpType { get; set; } = "Increased damage";
        public float powerUpEffect = 0.25f;

        public AudioClip collisionSound;


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
                if (collisionSound)
                {
                    AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
                }

                var player = other.GetComponent<Player.Player>();
                if (!player) return;

                IncreasedDamage = player.damage * powerUpEffect;
                player.damage += IncreasedDamage;
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
                if (GamePlayCanvas)
                    GamePlayCanvas.ShowPowerUpLefTime(PowerUpType, powerUpDuration - Timer);
             
                Timer += Time.deltaTime;

                if (Timer > powerUpDuration)
                {
                    var player = Player.GetComponent<Player.Player>();
                    if (!player) return;

                    IsPowerUpActive = false;
                    Destroy(gameObject);
                    Spawner.spawnAreas[ReservedArea].isActive = false;
                    IsAlive = false;
                  
                    if (GamePlayCanvas)
                        GamePlayCanvas.HidePowerUpLefTime(powerUpDuration - Timer);
                    player.damage -= IncreasedDamage;
                    player.ActivePowerUp = false;
                }
            }
        }
    }
}