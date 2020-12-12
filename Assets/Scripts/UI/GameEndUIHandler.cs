using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Application = UnityEngine.Application;

namespace Assets.Scripts.UI
{
    public class GameEndUIHandler : MonoBehaviour
    {
        public Text collectedCoins;
        public Text highestNUmberOfCollectedCoins;

        private void Start()
        {
            collectedCoins.text += PlayerPrefs.GetInt("CollectedCoins").ToString();
            if (PlayerPrefs.GetInt("CollectedCoins") > PlayerPrefs.GetInt("HighestNumberOfCollectedCoins"))
            {
                PlayerPrefs.SetInt("HighestNumberOfCollectedCoins", PlayerPrefs.GetInt("CollectedCoins"));
            }

            highestNUmberOfCollectedCoins.text += PlayerPrefs.GetInt("HighestNumberOfCollectedCoins").ToString();
        }


        public void StartGame()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }


        public void QuitButton()
        {
            Application.Quit();
        }

        public void ExitButton()
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}