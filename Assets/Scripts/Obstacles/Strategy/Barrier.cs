﻿using UnityEngine;

namespace Assets.Scripts.Obstacles.Strategy
{
    public class Barrier : MonoBehaviour
    {
        public float collisionDamage = 5f;
        public AudioSource CollisionSound { get; set; }

        private void Awake()
        {
            CollisionSound = GetComponent<AudioSource>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                if (CollisionSound)
                {
                    CollisionSound.Play();
                }

                var player = other.GetComponent<Player.Player>();
                if (!player) return;
                player.health -= collisionDamage;
            }
        }
    }
}