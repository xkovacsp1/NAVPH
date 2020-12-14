using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class HowToPlay : MonoBehaviour
    {

        public void BactToMainMenu()
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }


    }
}
