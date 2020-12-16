using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public float speed = 120f;
        public float turnSpeed = 180f;
        public float health = 100f;
        public float damage = 10f;
        public float collisionDamage = 10f;

        public int NumberOfCollectedCoins { get; set; }
        public string MovementAxisName { get; set; } = "Vertical";
        public string TurnAxisName { get; set; } = "Horizontal";
        public Rigidbody RigidBody { get; set; }
        public float MovementInputValue { get; set; }
        public float TurnInputValue { get; set; }
        public string PauseButton { get; set; } = "Pause";
        public string CancelButton { get; set; } = "Cancel";
        public bool ActivePowerUp { get; set; } = false;
        public bool GamePaused { get; set; }

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            MovementInputValue = 0f;
            TurnInputValue = 0f;
        }

        private void Update()
        {
            //pause game with player
            if (Input.GetButtonDown(PauseButton))
            {
                if (Time.timeScale == 1.0f)
                {
                    Time.timeScale = 0.0f;
                    GamePaused = true;
                }
                else
                {
                    Time.timeScale = 1.0f;
                    GamePaused = false;
                }
            }

            //exit to main menu
            if (Input.GetButtonDown(CancelButton))
            {
                SceneManager.LoadScene(sceneBuildIndex: 0);
            }

            //Die
            if (health <= 0.0)
            {
                PlayerPrefs.SetInt("CollectedCoins", NumberOfCollectedCoins);
                SceneManager.LoadScene(sceneBuildIndex: 2);
                return;
            }

            MovementInputValue = Input.GetAxis(MovementAxisName);
            TurnInputValue = Input.GetAxis(TurnAxisName);
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void Move()
        {
            if (RigidBody)
                RigidBody.velocity = transform.forward * MovementInputValue * speed * Time.deltaTime;
        }

        private void Turn()
        {
            float playerTurn = TurnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, playerTurn, 0f);
            if (RigidBody)
                RigidBody.MoveRotation(RigidBody.rotation * turnRotation);
        }
    }
}