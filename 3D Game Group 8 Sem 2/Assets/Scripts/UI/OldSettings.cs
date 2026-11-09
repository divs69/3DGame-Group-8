using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public Button SettingsButton;
    public Button ResumeButton;
    public GameObject SettingsPanel;
    public Button QuitButton;
    public Slider VolumeSlider;
    void Start()
    {
        SettingsPanel.SetActive(false);
        ResumeButton.onClick.AddListener(HideSettingsPanel);
        SettingsButton.onClick.AddListener(DisplaySettingsPanel);
        QuitButton.onClick.AddListener(QuitGame);
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        AudioListener.volume = savedVolume;
        VolumeSlider.value = savedVolume;
        VolumeSlider.onValueChanged.AddListener(ChangeVolume);
        PlayerPrefs.Save();
    }

    private void HideSettingsPanel()
    {
        SettingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void DisplaySettingsPanel()
    {
        SettingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}