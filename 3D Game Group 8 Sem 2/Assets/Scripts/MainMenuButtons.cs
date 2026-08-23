using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{

    public Button StartButton;
    public Button QuitButton;
    public GameObject UIPanel;

    void Start()
    {
        StartButton.onClick.AddListener(HidePanel);

        QuitButton.onClick.AddListener(Quit);
    }

    private void HidePanel()
    {

        SceneManager.LoadScene("LevelOne");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
