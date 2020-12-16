using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class HealthBarRotation : MonoBehaviour
    {
        public Camera CameraToLookAt { get; set; }

        private void Awake()
        {
            CameraToLookAt = GameObject.FindWithTag("Player").transform.GetChild(7).GetComponent<Camera>();
        }

        public void Update()
        {
            if (!CameraToLookAt) return;
            var cameraPosition = CameraToLookAt.transform.position;
            var movementOffset = cameraPosition - transform.position;
            movementOffset.x = movementOffset.z = 0.0f;
            transform.LookAt(cameraPosition - movementOffset);
            transform.Rotate(0, 180, 0);
        }
    }
}