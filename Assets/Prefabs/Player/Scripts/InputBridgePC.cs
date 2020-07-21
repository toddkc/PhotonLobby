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
    [SerializeField] private KeyCode interactKey = default;
    [SerializeField] private int mouseInteract = 0;

    private void Update()
    {
        MoveAxis = Input.GetAxis(moveAxis);
        StrafeAxis = Input.GetAxis(strafeAxis);
        PitchAxis = Input.GetAxis(pitchAxis);
        YawAxis = Input.GetAxis(yawAxis);
        Interact = Input.GetKey(interactKey) || Input.GetMouseButton(mouseInteract);
    }
}
