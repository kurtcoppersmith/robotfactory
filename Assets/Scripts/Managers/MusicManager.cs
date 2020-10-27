using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    public AudioSource basicAudioSource;
    public AudioClip panicMusic;

    public float fadeInTime = 3;
    public float timeUntilPanicMusic = 60f;

    new void Awake()
    {
        if (Instance != this)
        {
            Instance.basicAudioSource.clip = this.basicAudioSource.clip;
        }

        base.Awake();

        basicAudioSource.Play();
    }

    void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
