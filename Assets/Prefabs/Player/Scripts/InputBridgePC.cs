using UnityEngine;

/// <summary>
/// kbm input
/// </summary>

public class InputBridgePC : InputBridgeBase
{
    [SerializeField] private string moveAxis = "Vertical";
    [SerializeField] private string strafeAxis = "Horizontal";
    [SerializeField] private string pitchAxis = "Mouse Y";
    [SerializeField] private string yawAxis = "Mouse X";
    [SerializeField] private string swimAxis = "Swim";
    [SerializeField] private KeyCode interactKey = default;
    [SerializeField] private KeyCode jumpKey = default;
    [SerializeField] private KeyCode climbKey = default;
    [SerializeField] private int mouseInteract = 0;

    private void Update()
    {
        MoveAxis = canPlayerMove ? Input.GetAxis(moveAxis) : 0;
        StrafeAxis = canPlayerMove ? Input.GetAxis(strafeAxis) : 0;
        SwimAxis = canPlayerMove ? Input.GetAxis(swimAxis) : 0;
        PitchAxis = Input.GetAxis(pitchAxis);
        YawAxis = Input.GetAxis(yawAxis);
        Interact = Input.GetKey(interactKey) || Input.GetMouseButton(mouseInteract);
        Jump = canPlayerMove ? Input.GetKeyDown(jumpKey) : false;
        Climb = canPlayerMove ? Input.GetKey(climbKey) : false;
    }
}
