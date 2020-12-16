using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class HowToPlay : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}