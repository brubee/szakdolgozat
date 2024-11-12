using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    public GameObject countdownTimer, gameManager;
    public bool winConditionMet = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (countdownTimer.GetComponent<CountdownTimer>().remainingTime < 5 && countdownTimer.GetComponent<CountdownTimer>().remainingTime > 0)
            {
                winConditionMet = true;
                gameManager.GetComponent<GameManager>().WinGame();
            }
        }
    }
}
