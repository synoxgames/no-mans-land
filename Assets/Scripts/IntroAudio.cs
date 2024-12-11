using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudio : MonoBehaviour
{
    public AudioClip[] lightningClips;
    AudioSource audSource;

    public void Awake() {
        audSource = GetComponent<AudioSource>();
    }

    public void PlayLightningClip() {
        audSource.PlayOneShot(lightningClips[Random.Range(0, lightningClips.Length)]);
    }
}
