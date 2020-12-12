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

        void Update()
        {
            var v = CameraToLookAt.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(CameraToLookAt.transform.position - v);
            transform.Rotate(0, 180, 0);
        }
    }
}