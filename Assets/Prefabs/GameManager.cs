using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this handles the logic for gameplay - it's a mess at the moment while I figure stuff out...
/// TODO: this needs to be made much more event driven
/// TODO: this will need to be made into an abstract class for multiple different games
/// </summary>

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    [SerializeField] private int maxScore = 1;
    [SerializeField] private float respawnTime = 5;
    [SerializeField] private GameObject avatar = default;
    [SerializeField] private GameEvent startGameEvent = default;
    [SerializeField] private GameEvent stopGameEvent = default;
    [SerializeField] private GameEvent displayMessageEvent = default;
    [SerializeField] private List<Team> teams = new List<Team>();

    private enum GameState
    {
        initializing = 0,
        pregame = 1,
        running = 2,
        postgame = 3
    }

    // return the next team for assigning players
    private int GetNextTeam()
    {
        int[] _teamSizes = CustomRoomProperties.GetTeams(PhotonNetwork.CurrentRoom);
        int _team = 0;
        int _min = _teamSizes[0];

        for (int i = 0; i < _teamSizes.Length; i++)
        {
            if (_teamSizes[i] < _min)
            {
                _min = _teamSizes[i];
                _team = i;
            }
        }

        return _team;
    }

    // return a material based on input team
    public static Material GetMaterial(Team team)
    {
        return team.teamColor;
    }

    // return a spawnpoint based on input team
    private Vector3 GetSpawn(Team team)
    {
        var _team = instance.teams[team.teamNumber];
        Vector3 pos = _team.teamSpawn.position;
        BoxCollider col = _team.teamSpawn.GetComponent<BoxCollider>();

        if (col != null)
        {
            for (int i = 10; i >= 0; i--)
            {
                pos.x = Random.Range(col.bounds.min.x, col.bounds.max.x);
                pos.z = Random.Range(col.bounds.min.z, col.bounds.max.z);
                if (col.bounds.Contains(pos)) break;
            }
        }

        return pos;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PUN_Events.StartGameEventCode)
        {
            startGameEvent.Raise();
            InputBridgeBase.ToggleMovement(true);
            PlayerPrefs.SetString("message", "Start!");
            displayMessageEvent.Raise();
        }
        else if (eventCode == PUN_Events.StopGameEventCode)
        {
            stopGameEvent.Raise();
            InputBridgeBase.ToggleMovement(true);
        }
    }

    // setup singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        var view = gameObject.AddComponent<PhotonView>();
        view.ViewID = 999;
    }

    private void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        InputBridgeBase.ToggleMovement(false);
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("custom room props updated: " + propertiesThatChanged.ToStringFull());
        // if game state goes from 0 to 1, spawn avatars
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.game) && (int)propertiesThatChanged[CustomRoomProperties.game] == 1)
        {
            SetupTeams();
        }
        // if game state goes from 1 to 2, start game (enable movement, start timers, etc)

        // if game state goes from 2 to 3, end game
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("custom player props updated: " + changedProps.ToStringFull());
    }

    private void SetupTeams()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            int _teamIndex = GetNextTeam();
            PhotonNetwork.CurrentRoom.AddToTeam(_teamIndex, 1);
            player.Value.SetTeam(_teamIndex);
            photonView.RPC("SpawnAvatar", player.Value, _teamIndex);
        }
        StartCoroutine(StartGameCountdown());
    }

    private IEnumerator StartGameCountdown()
    {
        yield return new WaitForSeconds(5);
        PUN_Events.StartGameEvent();
    }

    [PunRPC]
    private void SpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = GetSpawn(teams[teamIndex]);
        Transform _startPos = teams[teamIndex].teamSpawn;
        PhotonNetwork.Instantiate(avatar.name, _randomSpawn, _startPos.rotation);
    }
}


[System.Serializable]
public class Team
{
    public int teamNumber;
    public Material teamColor;
    public Transform teamSpawn;
}