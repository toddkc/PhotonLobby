using Photon.Pun;
using Photon.Realtime;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// handles all the room logic for PUN
/// </summary>
public class CustomNetworkRoom : MonoBehaviourPunCallbacks
{
    [Header("Settings")]
    [SerializeField] Text roomName = default;
    [SerializeField] Text playerName = default;

    [Header("Events")]
    [SerializeField] ScriptableObjectArchitecture.GameEvent joinRoomEvent = default;
    [SerializeField] ScriptableObjectArchitecture.GameEvent leaveRoomEvent = default;
    [SerializeField] ScriptableObjectArchitecture.GameEvent displayMessageEvent = default;

    [Header("Variables")]
    [SerializeField] private StringReference uiMessage = default;

    private int counter = 0;
    private bool joiningRandom = false;

    public static CustomNetworkRoom instance;

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

    // create a new room with a specific name
    public void CreateRoom()
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.InRoom) return;
        string roomname = PlayerPrefs.GetString("hostroom");
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.CreateRoom(roomname);
    }

    // join a room with a specific name
    public void JoinRoom()
    {
        joiningRandom = false;
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.InRoom) return;
        string roomname = PlayerPrefs.GetString("joinroom");
        if (string.IsNullOrEmpty(roomname)) return;
        PhotonNetwork.JoinRoom(roomname);
    }

    // attempt to join any room or else create one
    public void CreateOrJoinRandomRoom()
    {
        counter = 0;
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.InRoom) return;
        PhotonNetwork.JoinRandomRoom();
        joiningRandom = true;
    }

    // user-triggered leaving the current room
    public void LeaveRoom()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;
        PhotonNetwork.LeaveRoom();
    }

    // photon left room callback
    public override void OnLeftRoom()
    {
        //roomName.text = "";
        //playerName.text = "";
        leaveRoomEvent.Raise();
        // reset any state from prior game
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        uiMessage.Value = "you left the room";
        // if we aren't at the main menu go there
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
    }

    // photon player left callback
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        uiMessage.Value = otherPlayer.NickName + " has left the room";
    }

    // photon create room callback
    public override void OnCreatedRoom()
    {
        // for now only allow 4 player per room
        PhotonNetwork.CurrentRoom.MaxPlayers = 4;
    }

    // photon create room failed callback
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        uiMessage.Value = "create room failed: " + message;
    }

    // photon join room callback
    public override void OnJoinedRoom()
    {
        // set some debug text
        //roomName.text = PhotonNetwork.CurrentRoom.Name;
        //playerName.text = PhotonNetwork.NickName;
        joinRoomEvent.Raise();
        // reset any state from prior game
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        uiMessage.Value = "you joined a room";
        // spawn a network entity to setup voice
        CustomNetworkManager.SpawnNetworkEntity();
        // if master client proceed to lobby scene
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    // photon player joined room callback
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        uiMessage.Value = newPlayer.NickName + " has joined the room";
    }

    // photon join failed callback
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        uiMessage.Value = "join room failed: " + message;
    }

    // photon join random failed callback
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // it attempting to join a random room, try several times before creating
        if (joiningRandom)
        {
            if (counter < 10)
            {
                counter++;
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                joiningRandom = false;
                PhotonNetwork.CreateRoom(null);
            }
        }
    }
}