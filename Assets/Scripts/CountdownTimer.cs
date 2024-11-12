using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float remainingTime;
    public GameManager gameManager;
    public GameObject winningTrigger;


    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 10)
            {
                countdownText.color = Color.red;
            }
        }
        else if (remainingTime == 0)
        {
            remainingTime = 0;
        }
        else if (remainingTime < 0 && winningTrigger.GetComponent<WinningTrigger>().winConditionMet != true)
        {
            countdownText.text = "OUT OF TIME!";
            gameManager?.GetComponent<GameManager>().RanOutOfTimeGameOver();
        }
        
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
