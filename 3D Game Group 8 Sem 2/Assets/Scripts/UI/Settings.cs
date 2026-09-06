using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    [Header("UI Buttons & Panels")]
    public Button SettingsButton;
    public Button ResumeButton;
    public GameObject SettingsPanel;
    public Button QuitButton;
    public Slider VolumeSlider;

    public bool IsPaused {  get; private set; }

    void Start()
    {
        ResumeButton.onClick.AddListener(HideSettingsPanel);
        if (SettingsButton != null) SettingsButton.onClick.AddListener(DisplaySettingsPanel);
        QuitButton.onClick.AddListener(QuitGame);

        // Load saved volume settings
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        AudioListener.volume = savedVolume;
        if (VolumeSlider != null)
        {
            VolumeSlider.value = savedVolume;
            VolumeSlider.onValueChanged.AddListener(ChangeVolume);
        }

        HideSettingsPanel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                HideSettingsPanel();
            else
                DisplaySettingsPanel();
        }
    }

    public void HideSettingsPanel()
    {
        IsPaused = false;
        SettingsPanel.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DisplaySettingsPanel()
    {
        IsPaused = true;
        SettingsPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
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