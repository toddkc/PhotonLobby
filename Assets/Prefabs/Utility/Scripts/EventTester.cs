﻿using UnityEngine;

/// <summary>
/// this is used as a substitute for UI
/// </summary>

public class EventTester : MonoBehaviour
{
    [Header("Games")]
    public GameEvent loadGameEvent;
    public GameEvent unloadGameEvent;
    [Header("Room Events")]
    public GameEvent joinEvent;
    public GameEvent leaveEvent;
    [Header("Game Events")]
    public GameEvent startGameEvent;
    public GameEvent quitGameEvent;
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

        if (Input.GetKeyDown(loadGameKey)) loadGameEvent.Raise();
        if (OVRInput.Get(loadGameButton)) loadGameEvent.Raise();

        if (Input.GetKeyDown(unloadGameKey)) unloadGameEvent.Raise();
        if (OVRInput.Get(unloadGameButton)) unloadGameEvent.Raise();
    }
}