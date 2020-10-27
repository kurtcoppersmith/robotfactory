using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : SingletonMonoBehaviour<SoundEffectsManager>
{
    public AudioSource soundEffectSource;
    public List<AudioClip> audioClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> namedAudioClips = new Dictionary<string, AudioClip>();

    new void Awake()
    {
        base.Awake();

        for (int i = 0; i < audioClips.Count; i++)
        {
            namedAudioClips.Add(audioClips[i].name, audioClips[i]);
        }
    }

    public void Play(string audioClipName)
    {
        if(audioClipName == null || audioClipName == "")
        {
            return;
        }

        soundEffectSource.spatialize = false;
        soundEffectSource.PlayOneShot(namedAudioClips[audioClipName]);
    }
}
