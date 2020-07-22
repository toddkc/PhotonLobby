using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this is used for the host to sync a game scene for all guests
/// </summary>

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameEvent displayMessageEvent = default;
    [SerializeField] private GameEvent sceneLoadedEvent = default;
    [SerializeField] private GameEvent sceneUnloadedEvent = default;
    private bool isSceneLoaded = false;
    private int? currentGameScene = null;

    public static SceneLoader instance;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoadedCallback;
        SceneManager.sceneUnloaded += SceneUnloadedCallback;
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoadedCallback;
        SceneManager.sceneUnloaded -= SceneUnloadedCallback;
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void LeaveRoom()
    {
        Debug.Log("leave room");
        UnloadGameSceneEventResponse();
    }

    public void LoadGameScene(int scene)
    {
        if (!isSceneLoaded && PhotonNetwork.IsMasterClient)
        {
            PUN_Events.LoadLevelEvent(scene);
        }
    }

    public void UnloadGameScene()
    {
        if (isSceneLoaded && PhotonNetwork.IsMasterClient)
        {
            PUN_Events.UnloadLevelEvent();
        }
    }

    private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == currentGameScene)
        {
            sceneLoadedEvent.Raise();
            CustomPlayerProperties.UpdateProps<int>(PhotonNetwork.LocalPlayer, "loaded_scene", scene.buildIndex);
        }
    }

    private void SceneUnloadedCallback(Scene scene)
    {
        if (scene.buildIndex == currentGameScene)
        {
            sceneUnloadedEvent.Raise();
            isSceneLoaded = false;
            currentGameScene = null;
        }
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PUN_Events.LoadLevelEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int buildIndex = (int)data[0];
            LoadGameSceneEventResponse(buildIndex);
        }
        else if (eventCode == PUN_Events.UnloadLevelEventCode)
        {
            UnloadGameSceneEventResponse();
        }
    }

    private void LoadGameSceneEventResponse(int index)
    {
        if (!isSceneLoaded)
        {
            isSceneLoaded = true;
            currentGameScene = index;
            PlayerPrefs.SetString("message", "Loading Game...");
            displayMessageEvent.Raise();

            PhotonNetwork.LoadLevel(index);
        }
    }

    private void UnloadGameSceneEventResponse()
    {
        if (isSceneLoaded && currentGameScene != null)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}