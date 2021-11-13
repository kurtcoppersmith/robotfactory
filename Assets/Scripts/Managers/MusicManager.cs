using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    public AudioSource basicAudioSource;
    public AudioClip panicMusic;
    public AudioClip levelEndMusic;

    private AudioClip fadeTempMusic;
    public float volumeLevel = 1;

    public float fadeInTime = 3;
    public float timeUntilPanicMusic = 61f;

    public AudioMixer masterMixer;

    new void Awake()
    {
        if (Instance != this && Instance != null)
        {
            if ((Instance.basicAudioSource.clip.name != this.basicAudioSource.clip.name || Instance.fadeTempMusic.name != this.basicAudioSource.clip.name) && this.basicAudioSource.clip != null)
            {
                Instance.FadeInPlay(Instance.basicAudioSource, this.basicAudioSource.clip);
            }
        }
        base.Awake();
    }

    void Start()
    {
        if (Instance == this)
        {
            fadeTempMusic = basicAudioSource.clip;
            DontDestroyOnLoad(this.gameObject);

            //SetAudioLevels();

            if (basicAudioSource != null)
            {
                basicAudioSource.Play();
            }
        }
    }

    public void SetAudioLevels()
    {
        masterMixer.SetFloat("mastVol", GameManager.Instance.GetGameData().playerSettings.masterVolume);

        masterMixer.SetFloat("musVol", GameManager.Instance.GetGameData().playerSettings.musicVolume);

        masterMixer.SetFloat("sfxVol", GameManager.Instance.GetGameData().playerSettings.sfxVolume);

        if (basicAudioSource != null)
        {
            basicAudioSource.Play();
        }
    }

    void Update()
    {
        if (LevelManager.Instance != null)
        {
            if (!GameManager.Instance.hasEnded && GameManager.Instance.returnTime() <= timeUntilPanicMusic && fadeTempMusic != panicMusic && basicAudioSource.clip.name != panicMusic.name)
            {
                FadeInPlay(basicAudioSource, panicMusic);
            }

            if (GameManager.Instance.hasEnded && fadeTempMusic != levelEndMusic && basicAudioSource.clip.name != levelEndMusic.name)
            {
                FadeInPlay(basicAudioSource, levelEndMusic);
            }
        }

        
    }

    IEnumerator PlayFadeMusic()
    {
        yield return new WaitForSecondsRealtime(fadeInTime);
        
        DOTween.Kill(3);
        basicAudioSource.volume = volumeLevel;
        basicAudioSource.clip = fadeTempMusic;
        basicAudioSource.Play();
    }

    void FadeInPlay(AudioSource InstanceSource, AudioClip audioClip)
    {
        InstanceSource.DOKill();
        fadeTempMusic = audioClip;
        InstanceSource.DOFade(0, fadeInTime).SetId(3).SetUpdate(true);

        StartCoroutine(PlayFadeMusic());
    }
}
