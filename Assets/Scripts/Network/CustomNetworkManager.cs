namespace NetworkTutorial
{
    using NetworkTutorial.GameEvents;
    using Photon.Pun;
    using Photon.Realtime;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class CustomNetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("Prefabs")]
        [SerializeField] GameObject playerPrefab = default;

        [Header("Settings")]
        [SerializeField] Text roomName = default;
        [SerializeField] Text playerName = default;
        [SerializeField] bool printDebug = false;
        [SerializeField] bool spawnPlayerEntity = false;

        [Header("Events")]
        [SerializeField] GameEvent joinRoomEvent = default;
        [SerializeField] GameEvent leaveRoomEvent = default;
        [SerializeField] GameEvent disconnectEvent = default;
        [SerializeField] GameEvent connectEvent = default;
        [SerializeField] GameEvent displayMessageEvent = default;

        private int counter = 0;
        private bool joiningRandom = false;
        private GameObject networkPlayerInstance;

        public static CustomNetworkManager instance;

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
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.ConnectUsingSettings();

            PlayerPrefs.SetString("message", "connecting to PUN...");
            displayMessageEvent.Raise();
        }

        private void CustomStart(Scene oldScene, Scene newScene)
        {
            StartCoroutine(GetStatus());
        }

        private IEnumerator GetStatus()
        {
            yield return null;
            Debug.Log("custom network manager START");
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

        public override void OnEnable()
        {
            base.OnEnable();
            SceneManager.activeSceneChanged += CustomStart;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.activeSceneChanged -= CustomStart;
        }

        // create a new room with a specific name
        public void CreateRoom()
        {
            joiningRandom = false;
            if (!PhotonNetwork.IsConnected || PhotonNetwork.InRoom) return;
            string roomname = PlayerPrefs.GetString("hostroom");
            if (string.IsNullOrEmpty(roomname)) return;
            PhotonNetwork.CreateRoom(roomname);
        }

        // join a room with a specific name
        public void JoinRoom()
        {
            joiningRandom = false;
            if (!PhotonNetwork.IsConnected || PhotonNetwork.InRoom) return;
            string roomname = PlayerPrefs.GetString("joinroom");
            if (string.IsNullOrEmpty(roomname)) return;
            PhotonNetwork.JoinRoom(roomname);
        }

        // attempt to join any room or else create one
        public void CreateOrJoinRandomRoom()
        {
            counter = 0;
            if (!PhotonNetwork.IsConnected || PhotonNetwork.InRoom) return;
            PhotonNetwork.JoinRandomRoom();
            joiningRandom = true;
        }

        // user-triggered leaving the current room
        public void LeaveRoom()
        {
            if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;
            PhotonNetwork.LeaveRoom();
        }

        // user-triggered PUN disconnect
        public void Disconnect()
        {
            if (!PhotonNetwork.IsConnected) return;
            PhotonNetwork.Disconnect();
        }

        private void SpawnPlayer(Player player)
        {
            networkPlayerInstance = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(networkPlayerInstance);
        }

        public override void OnConnectedToMaster()
        {
            if (printDebug) Debug.Log("connected to PUN");
            connectEvent.Raise();
            PlayerPrefs.SetString("message", "connected to PUN");
            displayMessageEvent.Raise();
        }

        public override void OnLeftRoom()
        {
            if (printDebug) Debug.Log("left room");
            roomName.text = "";
            playerName.text = "";
            leaveRoomEvent.Raise();
            CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
            PlayerPrefs.SetString("message", "you left the room");
            displayMessageEvent.Raise();

            SceneManager.LoadSceneAsync(0);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (printDebug) Debug.Log(otherPlayer.NickName + " has left the room");
            PlayerPrefs.SetString("message", otherPlayer.NickName + " has left the room");
            displayMessageEvent.Raise();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (printDebug) Debug.Log("disconnected from server");
            disconnectEvent.Raise();
            PlayerPrefs.SetString("message", "disconnect from PUN");
            displayMessageEvent.Raise();

            SceneManager.LoadSceneAsync(0);
        }

        public override void OnCreatedRoom()
        {
            if (printDebug) Debug.Log("created room: " + PhotonNetwork.CurrentRoom.Name);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("create room failed: " + message);
            PlayerPrefs.SetString("message", "create room failed: " + message);
            displayMessageEvent.Raise();
        }

        public override void OnJoinedRoom()
        {
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            playerName.text = PhotonNetwork.NickName;
            if (printDebug) Debug.Log("joined room: " + PhotonNetwork.CurrentRoom.Name);
            joinRoomEvent.Raise();
            CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
            PlayerPrefs.SetString("message", "you joined a room");
            displayMessageEvent.Raise();

            if (spawnPlayerEntity)
            {
                SpawnPlayer(PhotonNetwork.LocalPlayer);
            }

            SceneManager.LoadSceneAsync(1);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (printDebug) Debug.Log(newPlayer.NickName + "has joined the room");
            PlayerPrefs.SetString("message", newPlayer.NickName + " has joined the room");
            displayMessageEvent.Raise();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("join room failed: " + message);
            PlayerPrefs.SetString("message", "join room failed: " + message);
            displayMessageEvent.Raise();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (printDebug) Debug.Log("join random failed");
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
}