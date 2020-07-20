using NetworkTutorial.GameEvents;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceControls : MonoBehaviour
{
    Recorder recorder;
    [SerializeField] OVRInput.RawButton toggleButton = default;
    [SerializeField] GameEvent displayMessageEvent = default;

    private void Awake()
    {
        recorder = GetComponent<Recorder>();
    }

    private void Update()
    {
        if (OVRInput.GetDown(toggleButton))
        {
            recorder.DebugEchoMode = !recorder.DebugEchoMode;
            PlayerPrefs.SetString("message", "mic echo on: " + recorder.DebugEchoMode);
            displayMessageEvent.Raise();
        }
    }
}
