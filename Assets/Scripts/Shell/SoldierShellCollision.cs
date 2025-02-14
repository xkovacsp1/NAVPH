﻿using Assets.Scripts.Enemy.Strategy;
using UnityEngine;

namespace Assets.Scripts.Shell
{
    public class SoldierShellCollision : MonoBehaviour
    {
        public GameObject smallExplosion;

        public Rigidbody ShellRigidBody { get; set; }
        public float SoldierDamage { get; set; }


        private void Awake()
        {
            ShellRigidBody = gameObject.GetComponent<Rigidbody>();
            SoldierDamage = GameObject.FindWithTag("EnemySoldier").GetComponent<EnemySoldier>().fireDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag($"Player"))
            {
                var player = other.GetComponent<Player.Player>();
                if (!player) return;
                player.health -= SoldierDamage;
                ShowExplosion();
                Destroy(gameObject);
            }
            else
            {
                ShowExplosion();
                Destroy(gameObject);
            }
        }

        private void ShowExplosion()
        {
            if (!smallExplosion || !ShellRigidBody) return;
            var explosion = Instantiate(smallExplosion, ShellRigidBody.position, ShellRigidBody.rotation);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(explosion.gameObject, explosion.GetComponent<ParticleSystem>().main.duration);
        }
    }
}