
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI
{
    public class GamePlay: MonoBehaviour
    {
        public GameObject pauseMessage;
        public GameObject abilityScoreHeader;
        public GameObject abilityTimeLeft;
        public Text abilityTimeLeftText;
        public Text abilityScoreHeaderText;
        public Text textHealth;
        public Text coinNumber;
        public Text timeLeftText;


        public void ShowPauseMessage()
        {
            pauseMessage.SetActive(true);
        }

        public void ShowHealthScore(float score)
        {
            textHealth.text = score.ToString(CultureInfo.CurrentCulture);
        }


        public void ShowCoinScore(float score)
        {
            coinNumber.text = score.ToString(CultureInfo.CurrentCulture);
        }


        public void HidePauseMessage()
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
