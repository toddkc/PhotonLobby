using NetworkTutorial.GameEvents;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    [Header("Games")]
    public GameEvent tictactoeEvent;
    [Header("Room Events")]
    public GameEvent joinEvent;
    public GameEvent leaveEvent;
    [Header("Game Events")]
    public GameEvent startGameEvent;
    public GameEvent quitGameEvent;
    [Header("VR Buttons")]
    public OVRInput.RawButton joinButton;
    public OVRInput.RawButton leaveButton;
    public OVRInput.RawButton tictactoeButton;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) joinEvent.Raise();
        if (OVRInput.Get(joinButton)) joinEvent.Raise();

        if (Input.GetKeyDown(KeyCode.L)) leaveEvent.Raise();
        if (OVRInput.Get(leaveButton)) leaveEvent.Raise();

        if (Input.GetKeyDown(KeyCode.T)) tictactoeEvent.Raise();
        if (OVRInput.Get(tictactoeButton)) tictactoeEvent.Raise();
    }
}
