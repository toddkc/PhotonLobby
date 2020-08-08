using UnityEngine;

public static class PlayerPrefsManager
{
    //todo: player nickname
    //todo: room name
    //todo: room password

    public const string playMusic = "PlayMusic";
    public const string musicVolume = "MusicVolume";
    public const string muteMic = "MuteMic";
    public const string echoMic = "EchoMic";
    public const string snapAmount = "SnapAmount";

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

    public static bool MuteMic
    {
        get
        {
            if (!PlayerPrefs.HasKey(muteMic))
            {
                PlayerPrefs.SetInt(muteMic, 0);
            }
            return PlayerPrefs.GetInt(muteMic) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(muteMic, value ? 1 : 0);
        }
    }

    public static bool EchoMic
    {
        get
        {
            if (!PlayerPrefs.HasKey(echoMic))
            {
                PlayerPrefs.SetInt(echoMic, 0);
            }
            return PlayerPrefs.GetInt(echoMic) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(echoMic, value ? 1 : 0);
        }
    }

    public static float SnapAmount
    {
        get
        {
            if (!PlayerPrefs.HasKey(snapAmount))
            {
                PlayerPrefs.SetFloat(snapAmount, 45.0f);
            }
            return PlayerPrefs.GetFloat(snapAmount);
        }
        set
        {
            PlayerPrefs.SetFloat(snapAmount, value);
        }
    }
}