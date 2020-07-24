﻿using UnityEngine;

/// <summary>
/// oculus input
/// </summary>

public class InputBridgeVR : InputBridgeBase
{
    [SerializeField] private OVRInput.RawButton leftInteractButton;
    [SerializeField] private OVRInput.RawButton rightInteractButton;

    private void Update()
    {
        MoveAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        StrafeAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        YawAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        Interact = OVRInput.Get(leftInteractButton) || OVRInput.Get(rightInteractButton);
    }
}