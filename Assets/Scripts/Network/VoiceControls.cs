using NetworkTutorial.GameEvents;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceControls : MonoBehaviour
{
    Recorder recorder;
    [SerializeField] OVRInput.RawButton toggleButtonOculus = default;
    [SerializeField] KeyCode toggleKey = default;
    [SerializeField] GameEvent displayMessageEvent = default;

    public static VoiceControls instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            recorder = GetComponent<Recorder>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) || OVRInput.GetDown(toggleButtonOculus))
        {
            recorder.DebugEchoMode = !recorder.DebugEchoMode;
            PlayerPrefs.SetString("message", "mic echo on: " + recorder.DebugEchoMode);
            displayMessageEvent.Raise();
        }
    }
}
