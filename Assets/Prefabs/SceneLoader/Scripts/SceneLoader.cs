using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this is used for the host to sync a game scene for all guests
/// </summary>

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

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

    // master client will change to game scene and close room
    public void LoadGameScene(int scene)
    {
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex != scene)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(scene);
        }
    }

    // using player prefs to store desired game scene
    // TODO: change to scriptableobject global variable
    public void LoadGameSceneUsingPrefs()
    {
        if (!PhotonNetwork.IsMasterClient || !PlayerPrefs.HasKey("game")) return;
        int index = PlayerPrefs.GetInt("game");
        LoadGameScene(index);
    }

    // master client will load back to the lobby and reopen the room
    public void UnloadGameScene()
    {
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().buildIndex != 1)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.LoadLevel(1);
        }
    }
}