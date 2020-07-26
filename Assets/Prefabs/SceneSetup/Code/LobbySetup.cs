﻿using Photon.Pun;
using UnityEngine;

/// <summary>
/// experimenting with different lobby layouts so I'd like to be able to spawn everything via code
/// </summary>

public class LobbySetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isVR = false;

    [Header("Shared Prefabs")]
    public GameObject uiPlayerDisplay;
    public GameObject uiHostPanel;
    public GameObject uiGuestPanel;

    [Header("PC Prefabs")]
    public GameObject uiMessagesPC;
    public GameObject avatarPC;

    [Header("VR Prefabs")]
    public GameObject uiMessagesVR;
    public GameObject avatarVR;

    private void Start()
    {
        Instantiate(uiPlayerDisplay);
        Instantiate(uiHostPanel);
        Instantiate(uiGuestPanel);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR) return;
        Instantiate(uiMessagesVR);
        PhotonNetwork.Instantiate(avatarVR.name,Vector3.zero,Quaternion.identity);
    }

    private void SetupPC()
    {
        if (isVR) return;
        Instantiate(uiMessagesPC);
        PhotonNetwork.Instantiate(avatarPC.name, Vector3.zero, Quaternion.identity);
    }
}