using UnityEngine;

namespace Assets.Scripts.Player
{
    public class TankShooting : MonoBehaviour
    {
        public Transform fireTransform;
        public float minLaunchForce = 15f;
        public float maxLaunchForce = 30f;
        public float maxChargeTime = 0.75f;
        public float nextFire = 1.0f;
        public Rigidbody playerShell;
        public GameObject fireExplosion;

        public float FireTimer { get; set; }
        public string FireButton { get; set; } = "Fire";
        public float CurrentLaunchForce { get; set; }
        public float ChargeSpeed { get; set; }
        public bool Fired { get; set; }

        private void Start()
        {
            FireTimer = nextFire;
            ChargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
            CurrentLaunchForce = minLaunchForce;
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
                if (FireTimer > nextFire)
                    Fire();
            }
        }

        private void Fire()
        {
            Fired = true;
            FireTimer = 0;
            if (!playerShell || !fireTransform) return;

            var shellInstance = Instantiate(playerShell, fireTransform.position, fireTransform.rotation);
            shellInstance.velocity = CurrentLaunchForce * fireTransform.forward;

            ShowFireExplosion();

            CurrentLaunchForce = minLaunchForce;
        }


        private void ShowFireExplosion()
        {
            if (!fireExplosion || !fireTransform) return;
            var explosion = Instantiate(fireExplosion, fireTransform.position, fireTransform.rotation);
            explosion.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(explosion.GetComponentInChildren<ParticleSystem>(),
                explosion.GetComponentInChildren<ParticleSystem>().main.duration);
        }
    }
}