using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;


public class MainMenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
         public Button StartButton;
         public Button QuitButton;
         public GameObject MainPanel;

    void Start()
    {
        StartButton.onClick.AddListener(OpenLevelOne);
        QuitButton.onClick.AddListener(QuitGame);
    }
   

    // Update is called once per frame
    void OpenLevelOne()
    {
        SceneManager.LoadScene("LevelOne");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
