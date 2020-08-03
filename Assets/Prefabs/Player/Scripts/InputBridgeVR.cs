using UnityEngine;

/// <summary>
/// oculus input
/// </summary>

public class InputBridgeVR : InputBridgeBase
{
    [SerializeField] private OVRInput.RawButton leftInteractButton = default;
    [SerializeField] private OVRInput.RawButton rightInteractButton = default;
    [SerializeField] private OVRInput.RawAxis2D moveAxis = default;
    [SerializeField] private OVRInput.RawAxis2D strafeAxis = default;
    [SerializeField] private OVRInput.RawButton jumpKey = default;
    [SerializeField] private OVRInput.RawButton climbKey = default;

    private void Update()
    {
        MoveAxis = canPlayerMove ? OVRInput.Get(moveAxis).y : 0;
        StrafeAxis = canPlayerMove ? OVRInput.Get(strafeAxis).x : 0;
        Interact = OVRInput.Get(leftInteractButton) || OVRInput.Get(rightInteractButton);
        Jump = canPlayerMove ? OVRInput.GetDown(jumpKey) : false;
        Climb = canPlayerMove ? OVRInput.Get(climbKey) : false;
    }
}
