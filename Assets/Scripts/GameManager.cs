using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player, wendigo, gameOverMenu, ranOutOfTimeMenu, winTrigger, cutsceneObject, winScreen, countdownTimer;
    private bool isGamePaused;
    public Animator cutsceneAnimator;

    void Start()
    {
        //Main Menu Scene
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            player.GetComponent<PlayerController>().enabled = false;
            wendigo.GetComponentInChildren<EnemyController>().enabled = false;
        }

        //Game Scene
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            player.GetComponent<PlayerController>().enabled = true;
            wendigo.GetComponentInChildren<EnemyController>().enabled = true;
        }
    }

    public void WinGame()
    {
        if (winTrigger)
        {
            cutsceneAnimator.SetBool("winningConditionsMet", true);

            isGamePaused = true;
            countdownTimer.SetActive(false);
            winScreen.SetActive(true);

            player.GetComponent<PlayerController>().enabled = false;
            wendigo.GetComponent<EnemyController>().enabled = false;
        }
    }

    public void RanOutOfTimeGameOver()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        ranOutOfTimeMenu.SetActive(true);

        if (isGamePaused && gameObject != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        player.GetComponent<PlayerController>().enabled = false;
        wendigo.GetComponentInChildren<EnemyController>().enabled = false;
    }

    public void GameOver()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);

        if (isGamePaused && gameObject != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        player.GetComponent<PlayerController>().enabled = false;
        wendigo.GetComponentInChildren<EnemyController>().enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
