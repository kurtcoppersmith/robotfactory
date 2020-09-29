using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float volVar = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchVar = 0.1f;

    private AudioSource source;

    public void SetSource(AudioSource src)
    {
        source = src;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-volVar / 2f, volVar / 2f));
        source.pitch = pitch * (1 + Random.Range(-pitchVar / 2f, pitchVar / 2f));
        source.loop = true;
        source.Play();
    }
}

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject gObj = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(gObj.AddComponent<AudioSource>());
        }
        PlaySound("Assembly");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                Debug.Log("This");
                sounds[i].Play();
            }
        }
        Debug.LogWarning("AudioManager: No sound found with name " + _name);
    }
}