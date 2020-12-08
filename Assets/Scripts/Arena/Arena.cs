using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Arena
{
    public class Arena : MonoBehaviour
    {
        public float timeForArena = 60f;
        public Text timeLeftText;

        private void Awake()
        {
            timeLeftText.text = Math.Round(timeForArena).ToString();
           
        }

        void Update()
        {
            timeForArena -= Time.deltaTime;
            timeLeftText.text = Math.Round(timeForArena).ToString();

            if (timeForArena <= 0f)
            {
                SceneManager.LoadScene(sceneBuildIndex: 3);
            }
        }
    }
}
