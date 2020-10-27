using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    public AudioSource basicAudioSource;
    public AudioClip panicMusic;

    private AudioClip fadeTempMusic;
    public float volumeLevel = 1;

    public float fadeInTime = 3;
    public float timeUntilPanicMusic = 61f;

    new void Awake()
    {
        if (Instance != this && Instance != null)
        {
            if (Instance.basicAudioSource.clip.name != this.basicAudioSource.clip.name && this.basicAudioSource.clip != null)
            {
                Instance.FadeInPlay(Instance.basicAudioSource, this.basicAudioSource.clip);
            }
        }
        base.Awake();

        if (basicAudioSource != null)
        {
            basicAudioSource.Play();
        }
    }

    void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        if (LevelManager.Instance != null)
        {
            if (GameManager.Instance.returnTime() <= timeUntilPanicMusic && fadeTempMusic != panicMusic)
            {
                FadeInPlay(basicAudioSource, panicMusic);
            }
        }
    }

    void PlayFadeMusic()
    {
        basicAudioSource.volume = volumeLevel;
        basicAudioSource.clip = fadeTempMusic;
        basicAudioSource.Play();
    }

    void FadeInPlay(AudioSource InstanceSource, AudioClip audioClip)
    {
        fadeTempMusic = audioClip;
        InstanceSource.DOFade(0, fadeInTime);

        Invoke("PlayFadeMusic", fadeInTime);
    }
}
