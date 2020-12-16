using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TankShooting : MonoBehaviour
    {
        public Transform fireTransform;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;
        public float nextFire = 5.0f;
        public GameObject playerShell;
        public GameObject fireExplosion;

        public float FireTimer { get; set; }
        public string FireButton { get; set; } = "Fire";
        public float CurrentLaunchForce { get; set; }
        public float ChargeSpeed { get; set; }
        public bool Fired { get; set; }

        private void OnEnable()
        {
            CurrentLaunchForce = minLaunchForce;
        }

        private void Start()
        {
            FireTimer = nextFire;
            ChargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        }

        private void Update()
        {
            FireTimer += Time.deltaTime;
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
                if(FireTimer > nextFire)
                    Fire();
            }
        }

        private void Fire()
        {
            Fired = true;
            FireTimer = 0;
            if (!playerShell) return;
            var shellInstance = Instantiate(playerShell);
            var shellRigidBody = shellInstance.GetComponent<Rigidbody>();
            if (!shellRigidBody || !fireTransform) return;
            shellRigidBody.position = fireTransform.position;
            shellRigidBody.rotation = fireTransform.rotation;

            shellRigidBody.velocity = CurrentLaunchForce * fireTransform.forward;

            ShowFireExplosion();

            CurrentLaunchForce = minLaunchForce;
        }


        private void ShowFireExplosion()
        {
            if (!fireExplosion) return;
            var explosion = Instantiate(fireExplosion);
            var explosionRigidBody = explosion.GetComponent<Rigidbody>();
            explosionRigidBody.position = fireTransform.position;
            explosionRigidBody.rotation = fireTransform.rotation;
            explosion.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(explosion.GetComponentInChildren<ParticleSystem>(), explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        }


    }
}