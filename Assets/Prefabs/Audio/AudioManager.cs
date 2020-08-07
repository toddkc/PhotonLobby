using Lean.Pool;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private GameObject sourcePrefab = default;
    [SerializeField] private AudioSource defaultSource = default;
    [SerializeField] private AudioSource musicSource = default;
    [SerializeField] private AudioClip defaultMusic = default;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (defaultMusic)
        {
            PlayMusic(defaultMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (PlayerPrefsManager.PlayMusic)
        {
            musicSource.clip = clip;
            musicSource.volume = PlayerPrefsManager.MusicVolume;
            musicSource.Play();
        }
    }

    public void PlayClip(AudioClip clip)
    {
        defaultSource.PlayOneShot(clip);
    }

    public void PlayClipAtSource(AudioClip clip, Vector3 position)
    {
        var _poolsource = LeanPool.Spawn(sourcePrefab, position, Quaternion.identity);
        var _sourcecomp = _poolsource.GetComponent<AudioSource>();
        _sourcecomp.PlayOneShot(clip);
        LeanPool.Despawn(_poolsource, clip.length);
    }
}