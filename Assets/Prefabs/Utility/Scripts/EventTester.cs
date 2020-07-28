using UnityEngine;

/// <summary>
/// utility to simulate various UI events
/// </summary>

public class EventTester : MonoBehaviour
{
    //[Header("Games")]
    //public ScriptableObjectArchitecture.GameEvent loadGameEvent;
    //public ScriptableObjectArchitecture.GameEvent unloadGameEvent;
    [Header("Room Events")]
    public ScriptableObjectArchitecture.GameEvent joinEvent;
    public ScriptableObjectArchitecture.GameEvent leaveEvent;
    //[Header("Game Events")]
    //public ScriptableObjectArchitecture.GameEvent startGameEvent;
    //public ScriptableObjectArchitecture.GameEvent quitGameEvent;
    [Header("VR Buttons")]
    public OVRInput.RawButton joinButton;
    public OVRInput.RawButton leaveButton;
    public OVRInput.RawButton loadGameButton;
    public OVRInput.RawButton unloadGameButton;
    [Header("KeyCodes")]
    public KeyCode joinKey;
    public KeyCode leaveKey;
    public KeyCode loadGameKey;
    public KeyCode unloadGameKey;

    public static EventTester instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(joinKey)) joinEvent.Raise();
        if (OVRInput.Get(joinButton)) joinEvent.Raise();

        if (Input.GetKeyDown(leaveKey)) leaveEvent.Raise();
        if (OVRInput.Get(leaveButton)) leaveEvent.Raise();

        //if (Input.GetKeyDown(loadGameKey)) loadGameEvent.Raise();
        //if (OVRInput.Get(loadGameButton)) loadGameEvent.Raise();

        //if (Input.GetKeyDown(unloadGameKey)) unloadGameEvent.Raise();
        //if (OVRInput.Get(unloadGameButton)) unloadGameEvent.Raise();
    }
}
