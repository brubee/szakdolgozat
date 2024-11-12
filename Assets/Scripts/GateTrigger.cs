using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public Animator gateAnimator;
    public AudioClip gateAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gateAnimator.SetBool("isClosing", true);
            SoundManager.instance.playSoundClip(gateAudioClip, transform, 1f);
            gameObject.SetActive(false);
        }
    }
}
