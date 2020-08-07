using UnityEngine;

public static class PlayerPrefsManager
{
    //TODO:  lots of random playerprefs that need to be brought into here

    public const string playMusic = "PlayMusic";
    public const string musicVolume = "MusicVolume";

    public static bool PlayMusic
    {
        get
        {
            if (!PlayerPrefs.HasKey(playMusic))
            {
                PlayerPrefs.SetInt(playMusic, 1);
            }
            return PlayerPrefs.GetInt(playMusic) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(playMusic, value ? 1 : 0);
        }
    }
    public static float MusicVolume
    {
        get
        {
            if (!PlayerPrefs.HasKey(musicVolume))
            {
                PlayerPrefs.SetFloat(musicVolume, 0.05f);
            }
            return PlayerPrefs.GetFloat(musicVolume);
        }
        set
        {
            PlayerPrefs.SetFloat(musicVolume, value);
        }
    }
}