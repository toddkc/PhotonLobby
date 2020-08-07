using Photon.Pun;
using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;

public class ObstacleCourseGameMode : GameMode
{
    // not used, just reference for updating CustomRoomProperties.GameState
    protected enum GameState
    {
        pregame = 0,
        running = 1,
        postgame = 2
    }

    [SerializeField] private GameObject avatarPC = default;
    [SerializeField] private GameObject avatarVR = default;
    [SerializeField] private float startDelay = default;
    [SerializeField] private float lobbyDelay = default;
    [SerializeField] private GameEvent unloadGameEvent = default;

    private PhotonView view;
    private PhotonView localPlayerAvatarView;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        view = gameObject.AddComponent<PhotonView>();
        view.ViewID = 999;
    }

    private void Start()
    {
        ResetGame();
    }

    public override void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(StartGameCountdown());
    }

    public override void CheckGame()
    {
        if (!PhotonNetwork.IsMasterClient || !IsGameActive) return;
        var winningteam = GetComponent<GameScoring>().CheckScores();
        if (winningteam != -1)
        {
            var _props = new ExitGames.Client.Photon.Hashtable()
            {
                { CustomRoomProperties.game,  3 },
                {CustomRoomProperties.interest, winningteam }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(_props);
        }
    }

    public override void EndGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(ReturnToLobby());
    }

    protected override void ResetGame()
    {
        InputBridgeBase.ToggleMovement(false);
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    public override bool IsGameActive
    {
        get
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomRoomProperties.game, out object _state))
            {
                Debug.LogError("game state: " + CustomRoomProperties.GetGameState(PhotonNetwork.CurrentRoom));
                return (int)_state == 2;
            }
            return false;
        }
    }

    private IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(lobbyDelay);
        unloadGameEvent.Raise();
    }

    private IEnumerator StartGameCountdown()
    {
        yield return new WaitForSeconds(startDelay);
        CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 2);
    }

    public override void SetupAvatars()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            int _teamIndex = Team.GetNextTeam();
            PhotonNetwork.CurrentRoom.AddToTeam(_teamIndex, 1);
            player.Value.SetTeam(_teamIndex);
            view.RPC("RPCGameModeSpawnAvatar", player.Value, _teamIndex);
            Debug.LogError("player added to team: " + _teamIndex);
        }
    }

    [PunRPC]
    private void RPCGameModeSpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = Teams[teamIndex].GetSpawn();
        Transform _startPos = Teams[teamIndex].teamSpawn;
        GameObject _localavatar;
        if (isVrBuild.Value)
        {
            _localavatar = PhotonNetwork.Instantiate(avatarVR.name, _randomSpawn, _startPos.rotation);
        }
        else
        {
            _localavatar = PhotonNetwork.Instantiate(avatarPC.name, _randomSpawn, _startPos.rotation);
        }
        localPlayerAvatarView = _localavatar.GetComponentInChildren<PhotonView>();
    }
}