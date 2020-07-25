using Photon.Pun;
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
    [SerializeField] GameEvent disconnectEvent = default;
    [SerializeField] GameEvent connectEvent = default;
    [SerializeField] GameEvent displayMessageEvent = default;

    [Header("UI")]
    [SerializeField] GameObject uiPanel = default;

    public static CustomNetworkManager instance;

    public static void SpawnNetworkEntity()
    {
        var _player = PhotonNetwork.Instantiate(instance.networkEntityPrefab.name, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(_player);
    }

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

    public override void OnConnectedToMaster()
    {
        connectEvent.Raise();
        PlayerPrefs.SetString("message", "connected to PUN");
        displayMessageEvent.Raise();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectEvent.Raise();
        PlayerPrefs.SetString("message", "disconnect from PUN");
        displayMessageEvent.Raise();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
    }
}