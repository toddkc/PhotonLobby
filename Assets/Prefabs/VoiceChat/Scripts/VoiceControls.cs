using Photon.Voice.Unity;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// this will hold voice related options
/// </summary>

public class VoiceControls : MonoBehaviour
{
    Recorder recorder;
    [SerializeField] private StringReference uiMessage = default;

    public static VoiceControls instance;

    // setup singleton
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

    // toggle mic echo
    private void Update()
    {
        if (InputBridgeBase.instance.ToggleVoice)
        {
            recorder.DebugEchoMode = !recorder.DebugEchoMode;
            uiMessage.Value = "mic echo on: " + recorder.DebugEchoMode;
        }
    }
}
