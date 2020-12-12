using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public float speed = 120f;
        public float turnSpeed = 180f;
        public float health = 100f;
        public float damage = 10f;
        public Text textHealth;
        public Text coinNumber;
        public GameObject abilityScoreHeader;

        public GameObject abilityTimeLeft;
        public Text abilityTimeLeftText;

        public Text abilityScoreHeaderText;

        public int NumberOfCollectedCoins { get; set; }

        public string MovementAxisName { get; set; }
        public string TurnAxisName { get; set; }
        public Rigidbody RigidBody { get; set; }
        public float MovementInputValue { get; set; }
        public float TurnInputValue { get; set; }

        public bool ActivePowerUp { get; set; } = false;

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            MovementInputValue = 0f;
            TurnInputValue = 0f;
        }

        private void Start()
        {
            MovementAxisName = "Vertical";
            TurnAxisName = "Horizontal";
        }


        private void Update()
        {
            //Die
            if (health <= 0.0)
            {
                PlayerPrefs.SetInt("CollectedCoins", NumberOfCollectedCoins);
                SceneManager.LoadScene(sceneBuildIndex: 2);
            }


            if (textHealth && coinNumber)
            {
                textHealth.text = health.ToString(CultureInfo.CurrentCulture);
                coinNumber.text = NumberOfCollectedCoins.ToString();
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
            RigidBody.velocity = transform.forward * MovementInputValue * speed * Time.deltaTime;
        }

        private void Turn()
        {
            float playerTurn = TurnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, playerTurn, 0f);
            RigidBody.MoveRotation(RigidBody.rotation * turnRotation);
        }
    }
}