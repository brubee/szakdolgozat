using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsScreen, mainMenuScreen, controlsScreen;
    public AudioClip[] audioClips;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToSettings()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void FromSettings()
    {
        settingsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void ToControls()
    {
        mainMenuScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void FromControls()
    { 
        controlsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
