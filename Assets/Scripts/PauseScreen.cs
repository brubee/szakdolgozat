using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    private static bool isGamePaused = false;

    public GameObject pauseScreen, settingsScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isGamePaused && pauseScreen != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ToSettings()
    {
        Debug.Log("Clicked on Settings button");
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void FromSettings()
    {
        Debug.Log("Clicked on Back button");
        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void Retry()
    {
        Debug.Log("Clicked on Retry button");
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        Debug.Log("Clicked on Exit to Menu button");
        SceneManager.LoadScene(0);
    }
}
