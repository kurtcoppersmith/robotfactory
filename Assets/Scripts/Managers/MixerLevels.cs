using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MixerLevels : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        Debug.Log(GameManager.Instance.GetGameData().playerSettings.masterVolume);
        Debug.Log(GameManager.Instance.GetGameData().playerSettings.musicVolume);
        Debug.Log(GameManager.Instance.GetGameData().playerSettings.sfxVolume);

        masterMixer.SetFloat("mastVol", GameManager.Instance.GetGameData().playerSettings.masterVolume);
        masterSlider.SetValueWithoutNotify(GameManager.Instance.GetGameData().playerSettings.masterVolume);

        masterMixer.SetFloat("musVol", GameManager.Instance.GetGameData().playerSettings.musicVolume);
        musicSlider.SetValueWithoutNotify(GameManager.Instance.GetGameData().playerSettings.musicVolume);

        masterMixer.SetFloat("sfxVol", GameManager.Instance.GetGameData().playerSettings.sfxVolume);
        sfxSlider.SetValueWithoutNotify(GameManager.Instance.GetGameData().playerSettings.sfxVolume);
    }

    void Update()
    {
        if (masterSlider.value != GameManager.Instance.GetGameData().playerSettings.masterVolume)
        {
            masterSlider.value = GameManager.Instance.GetGameData().playerSettings.masterVolume;
        }

        if (musicSlider.value != GameManager.Instance.GetGameData().playerSettings.musicVolume)
        {
            musicSlider.value = GameManager.Instance.GetGameData().playerSettings.musicVolume;
        }

        if (sfxSlider.value != GameManager.Instance.GetGameData().playerSettings.sfxVolume)
        {
            sfxSlider.value = GameManager.Instance.GetGameData().playerSettings.sfxVolume;
        }
    }

    public void SetMasterLvl(float mastLvl)
    {
        masterMixer.SetFloat("mastVol", mastLvl);
        GameManager.Instance.GetGameData().playerSettings.masterVolume = mastLvl;
        GameManager.Instance.SaveGameData();

        Debug.Log(GameManager.Instance.GetGameData().playerSettings.masterVolume);
    }

    public void SetSfxLvl(float sfxLvl)
    {
        masterMixer.SetFloat("sfxVol", sfxLvl);
        GameManager.Instance.GetGameData().playerSettings.sfxVolume = sfxLvl;
        GameManager.Instance.SaveGameData();


        Debug.Log(GameManager.Instance.GetGameData().playerSettings.sfxVolume);
    }
    public void SetMusicLvl(float musicLvl)
    {
        masterMixer.SetFloat("musVol", musicLvl);
        GameManager.Instance.GetGameData().playerSettings.musicVolume = musicLvl;
        GameManager.Instance.SaveGameData();


        Debug.Log(GameManager.Instance.GetGameData().playerSettings.musicVolume);
    }
}
