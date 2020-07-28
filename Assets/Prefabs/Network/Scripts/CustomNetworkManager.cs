﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles all the server logic for PUN
/// </summary>
public class CustomNetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Prefabs")]
    [SerializeField] GameObject networkEntityPrefab = default;

    [Header("Events")]
    [SerializeField] ScriptableObjectArchitecture.GameEvent disconnectEvent = default;
    [SerializeField] ScriptableObjectArchitecture.GameEvent connectEvent = default;
    [SerializeField] ScriptableObjectArchitecture.GameEvent displayMessageEvent = default;

    [Header("UI")]
    [SerializeField] GameObject uiPanel = default;

    public static CustomNetworkManager instance;

    // spawns the non-avatar player, used to track player across scenes and setup voice
    public static void SpawnNetworkEntity()
    {
        var _player = PhotonNetwork.Instantiate(instance.networkEntityPrefab.name, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(_player);
    }

    // setup singleton
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

    // connect to photon on start
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PlayerPrefs.SetString("message", "connecting to PUN...");
        displayMessageEvent.Raise();
    }

    // used to update player messages on what is happening when scene changes
    private void CustomStart(Scene oldScene, Scene newScene)
    {
        uiPanel.SetActive(newScene.buildIndex < 2);
        StartCoroutine(GetStatus());
    }

    // used to update player messages on what is happening when scene changes
    private IEnumerator GetStatus()
    {
        yield return null;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();

            PlayerPrefs.SetString("message", "connecting to PUN...");
            displayMessageEvent.Raise();
        }
        else if (PhotonNetwork.InRoom)
        {
            PlayerPrefs.SetString("message", "joined room lobby");
            displayMessageEvent.Raise();
        }
        else
        {
            PlayerPrefs.SetString("message", "loading...");
            displayMessageEvent.Raise();
        }
    }
    
    // used to update player messages on what is happening when scene changes
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.activeSceneChanged += CustomStart;
    }
    
    // used to update player messages on what is happening when scene changes
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.activeSceneChanged -= CustomStart;
    }

    // user-triggered PUN disconnect
    public void Disconnect()
    {
        if (!PhotonNetwork.IsConnected) return;
        PhotonNetwork.Disconnect();
    }

    // photon connected callback
    public override void OnConnectedToMaster()
    {
        connectEvent.Raise();
        PlayerPrefs.SetString("message", "connected to PUN");
        displayMessageEvent.Raise();
    }

    // photon disconnected callback
    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectEvent.Raise();
        PlayerPrefs.SetString("message", "disconnect from PUN");
        displayMessageEvent.Raise();

        // if we aren't at the main menu scene go there
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}