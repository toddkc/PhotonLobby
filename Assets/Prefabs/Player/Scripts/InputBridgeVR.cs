using UnityEngine;

/// <summary>
/// oculus input
/// </summary>

public class InputBridgeVR : InputBridgeBase
{
    [SerializeField] private OVRInput.Axis2D moveAxis = default;
    [SerializeField] private OVRInput.Axis2D strafeAxis = default;
    [SerializeField] private OVRInput.Button jumpButton = default;
    [SerializeField] private OVRInput.Button voiceToggle = default;
    [SerializeField] private OVRInput.Button menuButton = default;
    [SerializeField] private OVRInput.Button scoreButton = default;
    [SerializeField] private OVRInput.Button interactButton = default;

    private void Update()
    {
        MoveAxis = canPlayerMove ? OVRInput.Get(moveAxis).y : 0;
        StrafeAxis = canPlayerMove ? OVRInput.Get(strafeAxis).x : 0;
        Jump = canPlayerMove ? OVRInput.GetDown(jumpButton) : false;
        ToggleVoice = OVRInput.Get(voiceToggle);
        Interact = OVRInput.Get(interactButton);
        Menu = OVRInput.Get(menuButton);
        Score = OVRInput.Get(scoreButton);

        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Debug.LogError("recenter");
            OVRManager.display.RecenterPose();
        }
    }
}
