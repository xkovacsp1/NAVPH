using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

namespace Assets.Scripts.UI
{
    public class MainMenuHandler : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }


        public void QuitButton()
        {
            Application.Quit();
        }

        public void HowToPlay()
        {
            SceneManager.LoadScene(sceneBuildIndex: 4);
        }
    }
}