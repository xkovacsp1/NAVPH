using Assets.Scripts.Shell;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TankShooting : MonoBehaviour
    {
        public Transform fireTransform;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;

        public string FireButton { get; set; }
        public float CurrentLaunchForce { get; set; }
        public float ChargeSpeed { get; set; }
        public bool Fired { get; set; }

        public GameObject playerShell;

        private void OnEnable()
        {
            CurrentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            FireButton = "Fire";
            ChargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            if (CurrentLaunchForce >= maxLaunchForce && !Fired)
            {
                CurrentLaunchForce = maxLaunchForce;
                Fire();
            }

            else if (Input.GetButtonDown(FireButton))
            {
                Fired = false;
                CurrentLaunchForce = minLaunchForce;
            }

            else if (Input.GetButton(FireButton) && !Fired)
            {
                CurrentLaunchForce += ChargeSpeed * Time.deltaTime;
            }

            else if (Input.GetButtonUp(FireButton) && !Fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            Fired = true;

            var shellInstance = Instantiate(playerShell);
            var shellRigidBody = shellInstance.GetComponent<Rigidbody>();
            if (!shellRigidBody) return;
            shellRigidBody.position = fireTransform.position;
            shellRigidBody.rotation = fireTransform.rotation;

            shellRigidBody.velocity = CurrentLaunchForce * fireTransform.forward;

            CurrentLaunchForce = minLaunchForce;
            //shellInstance.AddComponent<PlayerShellCollision>();
        }
    }
}