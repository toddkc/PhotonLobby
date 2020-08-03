using Photon.Pun;
using Photon.Realtime;
using ScriptableObjectArchitecture;
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
    [SerializeField] GameEvent disconnectEvent = default;
    [SerializeField] GameEvent connectEvent = default;
    [SerializeField] GameEvent displayMessageEvent = default;

    [Header("Variables")]
    [SerializeField] StringReference uiMessage = default;

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
        uiMessage.Value = "connecting to pun...";
    }

    // used to update player messages on what is happening when scene changes
    private void CustomStart(Scene oldScene, Scene newScene)
    {
        if(uiPanel) uiPanel.SetActive(newScene.buildIndex < 2);
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
            uiMessage.Value = "connecting to pun...";
        }
        else if (PhotonNetwork.InRoom && SceneManager.GetActiveScene().buildIndex < 2)
        {
            uiMessage.Value = "joined room";
        }
        else
        {
            uiMessage.Value = "loading...";
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
        uiMessage.Value = "connected to pun";
    }

    // photon disconnected callback
    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectEvent.Raise();
        uiMessage.Value = "disconnected from pun...";

        // if we aren't at the main menu scene go there
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}