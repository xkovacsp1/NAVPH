using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI
{
    public class GamePlay : MonoBehaviour
    {
        public GameObject pauseMessage;
        public GameObject abilityScoreHeader;
        public GameObject abilityTimeLeft;
        public Text abilityTimeLeftText;
        public Text abilityScoreHeaderText;
        public Text textHealth;
        public Text coinNumber;
        public Text timeLeftText;

        public Player.Player Player { get; set; }
        public Arena.Arena Arena { get; set; }

        private void Awake()
        {

            Player = GameObject.FindWithTag("Player").GetComponent<Player.Player>();
            Arena = GameObject.FindWithTag("Plane").GetComponent<Arena.Arena>();
            if (Arena)
                timeLeftText.text = Math.Round(Arena.timeForArena).ToString(CultureInfo.CurrentCulture);
        }

        private void Update()
        {
            if (!Player || !Arena) return;

            if (Player.GamePaused)
            {
                ShowPauseMessage();
            }
            else
            {
                HidePauseMessage();
            }

            ShowTimeLeftForArena();
            ShowHealthScore();
            ShowCoinScore();

        }

        private void ShowPauseMessage()
        {
            pauseMessage.SetActive(true);
        }

        private void ShowHealthScore()
        {
            if (Player.health < 0.0f)
                return;
            textHealth.text = Player.health.ToString(CultureInfo.CurrentCulture);
        }


        private void ShowCoinScore()
        {
            coinNumber.text = Player.NumberOfCollectedCoins.ToString(CultureInfo.CurrentCulture);
        }

        private void ShowTimeLeftForArena()
        {
            timeLeftText.text = Math.Round(Arena.timeForArena).ToString(CultureInfo.CurrentCulture);
        }


        private void HidePauseMessage()
        {
            pauseMessage.SetActive(false);
        }

       public void ShowPowerUpLefTime(string powerUpType, float timeLeft)
        {
            abilityScoreHeader.SetActive(true);
            abilityTimeLeft.SetActive(true);
            abilityScoreHeaderText.text = powerUpType;
            abilityTimeLeftText.text =
            Math.Round(timeLeft).ToString(CultureInfo.CurrentCulture);
        }

       public void HidePowerUpLefTime(float timeLeft)
       {
           abilityTimeLeftText.text =
               Math.Round(timeLeft).ToString(CultureInfo.CurrentCulture);
           abilityScoreHeader.SetActive(false);
           abilityTimeLeft.SetActive(false);
        }



    }
}
