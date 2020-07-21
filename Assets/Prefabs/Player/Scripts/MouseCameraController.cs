﻿using UnityEngine;

/// <summary>
/// controller for the pc player to look around
/// </summary>

public class MouseCameraController : MonoBehaviour
{
    [SerializeField]
    bool invert = false;
    [Range(0f, 90f)]
    [SerializeField]
    float upperLimit = 60f;
    [Range(0f, 90f)]
    [SerializeField]
    float lowerLimit = 60f;
    [SerializeField]
    float yawSpeed = 10f;
    [SerializeField]
    float pitchSpeed = 2f;

    private Transform thisTransform;
    private Transform camTransform;
    private float xAngle;
    private float yAngle;

    private void Awake()
    {
        thisTransform = transform;
        camTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        if(InputBridgePC.instance == null)
        {
            Debug.LogError("Mouse Cam Controller not setup properly!", this);
        }
        xAngle = camTransform.localRotation.eulerAngles.x;
        yAngle = thisTransform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        Pitch();
        Yaw();
    }

    // move cam up/down
    private void Pitch()
    {
        float input = invert ? InputBridgePC.instance.PitchAxis : -InputBridgePC.instance.PitchAxis;
        xAngle += input * pitchSpeed;
        xAngle = Mathf.Clamp(xAngle, -upperLimit, lowerLimit);
        camTransform.localRotation = Quaternion.Euler(new Vector3(xAngle, 0, 0));
    }

    // rotate gameobject
    private void Yaw()
    {
        float input = InputBridgePC.instance.YawAxis;
        yAngle += input * yawSpeed;
        thisTransform.localRotation = Quaternion.Euler(new Vector3(0,yAngle,0));
    }
}
