using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public float speed = 120f;                  // How fast the tank moves forward and back.
        public float turnSpeed = 180f;             // How fast the tank turns in degrees per
        public float health = 100f;
        public float damage = 10f;
        public Text textHealth;
        public Text coinNumber;
        public GameObject abilityScoreHeader;

        public GameObject abilityTimeLeft;
        public Text abilityTimeLeftText;
        public Text abilityScoreHeaderText;
        //public Text abilityTimeLeft;
        public int numberOfCollectedCoins;

        public string MovementAxisName { get; set; } // The name of the input axis for moving forward and back.
        public string TurnAxisName { get; set; } // The name of the input axis for turning.
        public Rigidbody RigidBody { get; set; } // Reference used to move the tank.
        public float MovementInputValue { get; set; } // The current value of the movement input.
        public float TurnInputValue { get; set; } // The current value of the turn input.

        public bool ActivePowerUp { get; set; } = false;
        private bool moving=false;
        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            //RigidBody.isKinematic = false;
            MovementInputValue = 0f;
            TurnInputValue = 0f;
        }

        private void OnDisable()
        {
           // RigidBody.isKinematic = true;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag($"LeftWall") || other.gameObject.CompareTag($"RightWall"))
            {
                health -= 10;
                //RigidBody.position += new Vector3(0.5f, 0.0f, 0.0f);
                //var movement = transform.forward * MovementInputValue * speed * Time.deltaTime;
                //RigidBody.MovePosition(RigidBody.position - movement);

                //if (health <= 0.0)
                //{
                //    Destroy(gameObject);
                //}
            }else if (other.gameObject.CompareTag($"Barrier"))
            {

                health -= 5;

            }
            else if (other.gameObject.CompareTag($"EnemyTiger"))
            {

                health -= 20;

            }

        }


        private void Start()
        {
            MovementAxisName = "Vertical";
            TurnAxisName = "Horizontal";
        }


        private void Update()
        {
            textHealth.text = health.ToString();
            coinNumber.text = numberOfCollectedCoins.ToString();
           
            MovementInputValue = Input.GetAxis(MovementAxisName);
            //if (MovementInputValue > 0.0f)
            //{
            //    moving = true;
            //}else if (MovementInputValue < 0.0f)
            //{

            //    moving = false;
            //}
            TurnInputValue = Input.GetAxis(TurnAxisName);
        }

        private void FixedUpdate()
        {
            //  if (moving)
            //  {
            //     Move(); 
            //  }
            //  else
            // {

            // RigidBody.velocity = Vector3.zero;
            // }
            Move();
            Turn();
        }

        private void Move()
        {
            //var movement = transform.forward  * MovementInputValue * speed * Time.deltaTime;
            RigidBody.velocity = transform.forward*MovementInputValue * speed * Time.deltaTime;



            //Vector3 movement = new Vector3(0, 0, MovementInputValue * speed);
            //// Returns a copy of vector with its magnitude clamped to maxLength
            //movement = Vector3.ClampMagnitude(movement, speed);

            //movement *= Time.deltaTime;
            //// Transforms direction from local space to world space.
            //movement = transform.TransformDirection(movement);
            ////RigidBody.AddForce(movement);
            //RigidBody.velocity = movement;





            //if (movement.z < -40.09f)
            //    movement.z += 0.1f;

            //if (movement.z > 39.64)
            //    movement.z -= 0.1f;

            //if (movement.x < -21.46)
            //    movement.x += 0.1f;

            //if (movement.x > 21.83f)
            //    movement.x -= 0.1f;
            //RigidBody.MovePosition(RigidBody.position + movement);

        }

        private void Turn()
        {
            float playerTurn = TurnInputValue * turnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, playerTurn, 0f);
            RigidBody.MoveRotation(RigidBody.rotation * turnRotation);
        }

    }
}
