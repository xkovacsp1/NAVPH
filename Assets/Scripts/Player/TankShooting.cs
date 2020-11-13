using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TankShooting : MonoBehaviour
    {
        public ShellPool shellPool;
        public Rigidbody shell;                   // Prefab of the shell.
        public Transform fireTransform;           // A child of the tank where the shells are spawned.
        public float minLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float maxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time
        public float maxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

        public string FireButton { get; set; }
        public float CurrentLaunchForce { get; set; }
        public float ChargeSpeed { get; set; }
        public bool Fired { get; set; }

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            CurrentLaunchForce = minLaunchForce;
        
        }

        private void Start()
        {
            // The fire axis is based on the player number.
            FireButton = "Fire";
            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            ChargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }
        private void Update()
        {
            // If the max force has been exceeded and the shell hasn't yet been launched...
            if (CurrentLaunchForce >= maxLaunchForce && !Fired)
            {
                // ... use the max force and launch the shell.
                CurrentLaunchForce = maxLaunchForce;
                Fire();
            }
            // Otherwise, if the fire button has just started being pressed...
            else if (Input.GetButtonDown(FireButton))
            {
                // ... reset the fired flag and reset the launch force.
                Fired = false;
                CurrentLaunchForce = minLaunchForce;
            }
            // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
            else if (Input.GetButton(FireButton) && !Fired)
            {
                // Increment the launch force and update the slider.
                CurrentLaunchForce += ChargeSpeed * Time.deltaTime;

          
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            else if (Input.GetButtonUp(FireButton) && !Fired)
            {
                // ... launch the shell.
                Fire();
            }
        }

        private void Fire()
        {
            // Set the fired flag so only Fire is only called once.
            Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            //Rigidbody shellInstance =
            //    Instantiate(shell, fireTransform.position, fireTransform.rotation);
            Rigidbody shellInstance = shellPool.Instantiate(fireTransform.position, fireTransform.rotation).GetComponent<Rigidbody>();
            
            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = CurrentLaunchForce * fireTransform.forward;

            // Reset the launch force.  This is a precaution in case of missing button events.
            CurrentLaunchForce = minLaunchForce;
        }
    }
}
