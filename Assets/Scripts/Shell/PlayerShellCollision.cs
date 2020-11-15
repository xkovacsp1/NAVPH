using System;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class PlayerShellCollision : MonoBehaviour, IShellBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"EnemyTiger") || other.gameObject.CompareTag($"Plane"))
            {
                Destroy(gameObject);
            }

        }
        public void Damage(GameObject other)
        {
           //bude to mozno velky switch

        }
    }
}
