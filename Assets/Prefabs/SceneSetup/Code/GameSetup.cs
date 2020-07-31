﻿using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// component used to spawn in required game scene items
/// this makes it easier to copy a game scene template
/// </summary>

public class GameSetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoolReference isVR = default;

    [Header("Shared Prefabs")]
    public GameObject uiPlayerDisplay;
    public GameObject uiGamePanel;

    [Header("PC Prefabs")]
    public GameObject uiMessagesPC;

    [Header("VR Prefabs")]
    public GameObject uiMessagesVR;

    private void Start()
    {
        Instantiate(uiPlayerDisplay);
        Instantiate(uiGamePanel);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR.Value) return;
        Instantiate(uiMessagesVR);
    }

    private void SetupPC()
    {
        if (isVR.Value) return;
        Instantiate(uiMessagesPC);
    }
}