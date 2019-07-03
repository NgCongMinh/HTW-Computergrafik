using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {

        public Button joinGameButton;

        public Button exitButton;

        public void Awake()
        {
            joinGameButton.onClick.AddListener(JoinGame);
            //exitButton.onClick.AddListener(Exit);
        }

        public void JoinGame()
        {
            Debug.Log("JOIN GAME");
            SceneManager.LoadScene(1);
        }

        public void Settings()
        {
            Debug.Log("SETTINGS");
        }

        public void Exit()
        {
            Debug.Log("EXIT");
            Application.Quit();
        }
        
    }
}