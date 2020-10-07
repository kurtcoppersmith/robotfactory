using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioMixerGroup audioMixerGroup;

    private AudioSource source;

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;

    public bool loop = false;
    public bool playOnAwake = false;

    public void setSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.playOnAwake = playOnAwake;
        source.loop = loop;
        source.outputAudioMixerGroup = audioMixerGroup;
    }

    public void Play()
    {
        source.Play();
    }
}

public class AudioManager : MonoBehaviour
{ 
    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());
        }
        PlaySound("Flo");
    }

    public void PlaySound(string _name)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
}
