using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuSettings : MonoBehaviour
{
    [SerializeField] private GameEvent settingsChanged = default;
    [SerializeField] private Toggle mute = default;
    [SerializeField] private Toggle echo = default;
    [SerializeField] private Toggle music = default;
    [SerializeField] private Slider volume = default;
    [SerializeField] private Slider snap = default;

    private void Start()
    {
        UpdateValues();
    }

    private void UpdateValues()
    {
        mute.isOn = PlayerPrefsManager.MuteMic;
        echo.isOn = PlayerPrefsManager.EchoMic;
        music.isOn = PlayerPrefsManager.PlayMusic;
        volume.value = PlayerPrefsManager.MusicVolume;
        snap.value = PlayerPrefsManager.SnapAmount;
    }

    public void ToggleMute()
    {
        PlayerPrefsManager.MuteMic = mute.isOn;
        settingsChanged.Raise();
    }

    public void ToggleEcho()
    {
        PlayerPrefsManager.EchoMic = echo.isOn;
        settingsChanged.Raise();
    }

    public void ToggleMusic()
    {
        PlayerPrefsManager.PlayMusic = music.isOn;
        settingsChanged.Raise();
    }

    public void SetVolume()
    {
        PlayerPrefsManager.MusicVolume = volume.value;
        settingsChanged.Raise();
    }

    public void SetSnap()
    {
        PlayerPrefsManager.SnapAmount = snap.value;
        settingsChanged.Raise();
    }
}