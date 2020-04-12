using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    AudioSource audio;
    public static BackgroundMusicController instance;
    public float FadeOutTime = 2f;
    public float FadeInTime = 5f;

    private void Awake()
    {
        instance = this;
        audio = GetComponent<AudioSource>();
        FadeInBackgroundMusic();
    }

    public void FadeOutBackgroundMusic()
    {
        StartCoroutine(AudioFadeScript.FadeOut(audio, FadeOutTime));

    }

    public void FadeInBackgroundMusic()
    {
        StartCoroutine(AudioFadeScript.FadeIn(audio, FadeInTime));
    }
}
