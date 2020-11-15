using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public float speed = 12f;                  // How fast the tank moves forward and back.
        public float turnSpeed = 180f;             // How fast the tank turns in degrees per
        public float cameraTurnSpeed = 60f;
        public int health = 100;
        public Text textHealth;
        public string MovementAxisName { get; set; } // The name of the input axis for moving forward and back.
        public string TurnAxisName { get; set; } // The name of the input axis for turning.
        public Rigidbody RigidBody { get; set; } // Reference used to move the tank.
        public float MovementInputValue { get; set; } // The current value of the movement input.
        public float TurnInputValue { get; set; } // The current value of the turn input.

        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            RigidBody.isKinematic = false;
            MovementInputValue = 0f;
            TurnInputValue = 0f;
        }

        private void OnDisable()
        {
            RigidBody.isKinematic = true;
        }

        //private void OnTriggerEnter(Collider other)
        //{

        //    if (other.gameObject.CompareTag($"EnemyTigerShell"))
        //    {
        //        health -= 10;
        //    }

        //}


        private void Start()
        {
            MovementAxisName = "Vertical";
            TurnAxisName = "Horizontal";
        }


        private void Update()
        {
            textHealth.text = health.ToString();
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
            var movement = transform.forward * MovementInputValue * speed * Time.deltaTime;
            RigidBody.MovePosition(RigidBody.position + movement);

        }

        private void Turn()
        {
            float playerTurn = TurnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, playerTurn, 0f);
            RigidBody.MoveRotation(RigidBody.rotation * turnRotation);
        }

    }
}
