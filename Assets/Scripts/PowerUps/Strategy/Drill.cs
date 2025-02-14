﻿using UnityEngine;

namespace Assets.Scripts.PowerUps.Strategy
{
    public class Drill : MonoBehaviour, IPowerUpsBehaviour
    {
        public float rotationSpeed = 60f;
        public float powerUpEffect = 10f;
        public AudioClip collisionSound;

        public bool IsAlive { get; private set; } = true;
        public int ReservedArea { get; set; }
        public Rigidbody RigidBody { get; set; }
        public PowerUpsSpawner Spawner { get; set; }

        private void Awake()
        {
            Spawner = GameObject.FindWithTag("PowerUpSpawner").GetComponent<PowerUpsSpawner>();
            RigidBody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag($"Player")) return;

            if (collisionSound && RigidBody)
            {
                AudioSource.PlayClipAtPoint(collisionSound, RigidBody.position);
            }

            var player = other.GetComponent<Player.Player>();
            if (!player) return;

            player.health += powerUpEffect;
            if (Spawner)
                Spawner.spawnAreas[ReservedArea].isActive = false;
            IsAlive = false;
            Destroy(gameObject);
        }

        public void Move()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        public bool IsActive()
        {
            return IsAlive;
        }

        public void TakeEffect()
        {
        }
    }
}