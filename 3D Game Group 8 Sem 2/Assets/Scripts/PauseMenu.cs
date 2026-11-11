using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;
    private bool grimoireCollected;

    private void OnEnable()
    {
        TutorialEvents.GrimoireCollected += UnlockGrimoire;
    }

    private void OnDisable()
    {
        TutorialEvents.GrimoireCollected -= UnlockGrimoire;
    }

    private void UnlockGrimoire()
    {
        grimoireCollected = true;
    }
    public Button nextbutton;
    public Button prevbutton;
    public Button booktab;
    public GameObject page1and2;
    public GameObject page3and4;

    public FPController FPControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenu.SetActive(false);
        page3and4.SetActive(false);
        nextbutton.onClick.AddListener(NextPage);
        prevbutton.onClick.AddListener(PreviousPage);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused && Keyboard.current != null)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame)
                NextPage();
            if (Keyboard.current.aKey.wasPressedThisFrame)
                PreviousPage();
        }
    }

    public void PauseGame()
    {
        if (!grimoireCollected || isPaused)
            return;

        if (Inventory2.instance != null && Inventory2.instance.IsOpen)
            Inventory2.instance.SetInventoryOpen(false);

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Disable mouse look script
        if (FPControllerScript != null)
        {
            FPControllerScript.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // re enable mouse look script
        if (FPControllerScript != null)
        {
            FPControllerScript.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void NextPage()
    {
        page1and2.SetActive(false);
        page3and4.SetActive(true);
        Debug.Log("the next button was pressed");

    }

    public void PreviousPage()
    {
        page1and2.SetActive(true);
        page3and4.SetActive(false);
        Debug.Log("the previous button was pressed");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
