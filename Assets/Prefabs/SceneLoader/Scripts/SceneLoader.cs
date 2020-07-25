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

    public void LoadGameScene(int scene)
    {
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex != scene)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(scene);
        }
    }

    public void LoadGameSceneUsingPrefs()
    {
        if (!PhotonNetwork.IsMasterClient || !PlayerPrefs.HasKey("game")) return;
        int index = PlayerPrefs.GetInt("game");
        LoadGameScene(index);
    }

    public void UnloadGameScene()
    {
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex != 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.LoadLevel(1);
        }
    }
}