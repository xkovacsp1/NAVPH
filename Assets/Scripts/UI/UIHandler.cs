using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;

namespace Assets.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {
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
