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

    [SerializeField] private int maxScore = 3;
    [SerializeField] private float respawnTime = 5;
    [SerializeField] private GameObject avatar = default;
    [SerializeField] private ScriptableObjectArchitecture.GameEvent startGameEvent = default;
    [SerializeField] private ScriptableObjectArchitecture.GameEvent stopGameEvent = default;
    [SerializeField] private ScriptableObjectArchitecture.GameEvent displayMessageEvent = default;
    [SerializeField] private List<Team> teams = new List<Team>();
    private Team winningTeam;

    // what state is our game in
    protected enum GameState
    {
        initializing = 0,
        pregame = 1,
        running = 2,
        postgame = 3
    }

    protected virtual void Awake()
    {
        Debug.Log("game manager awake");
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        var view = gameObject.AddComponent<PhotonView>();
        view.ViewID = 999;
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

    protected virtual void Start()
    {
        ResetGame();
    }

    protected virtual void ResetGame()
    {
        InputBridgeBase.ToggleMovement(false);
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        winningTeam = null;
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    protected virtual void OnEvent(EventData photonEvent)
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

    // return the next team for assigning players
    protected virtual int GetNextTeam()
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

    // return a spawnpoint based on input team
    protected virtual Vector3 GetSpawn(Team team)
    {
        Vector3 pos = team.teamSpawn.position;
        BoxCollider col = team.teamSpawn.GetComponent<BoxCollider>();

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

    protected virtual void SetupTeams()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
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
    protected virtual void SpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = GetSpawn(teams[teamIndex]);
        Transform _startPos = teams[teamIndex].teamSpawn;
        PhotonNetwork.Instantiate(avatar.name, _randomSpawn, _startPos.rotation);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("custom room props updated: " + propertiesThatChanged.ToStringFull());
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.game) && (int)propertiesThatChanged[CustomRoomProperties.game] == 1)
        {
            SetupTeams();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("custom player props updated: " + changedProps.ToStringFull());
    }

    public static bool IsGameActive
    {
        get
        {
            if (instance == null) return false;
            instance.CheckScores();
            if (instance.winningTeam != null || CustomRoomProperties.GetGameState(PhotonNetwork.CurrentRoom) != 2)
            {
                return false;
            }
            return true;
        }
    }

    private void CheckScores()
    {
        int[] scores = PhotonNetwork.CurrentRoom.GetScores();
        for (int i = 0; i < teams.Count; i++)
        {
            if (scores[i] >= maxScore)
            {
                winningTeam = teams[i];
                CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 2);
                break;
            }
        }
    }

    public static void PlayerScored(Player player, int value)
    {
        if (!PhotonNetwork.IsMasterClient || instance == null || !IsGameActive) return;
        // add score to player
        CustomPlayerProperties.SetScore(player, value);
        // add score to player team
        var team = CustomPlayerProperties.GetTeam(player);
        CustomRoomProperties.AddScore(PhotonNetwork.CurrentRoom, team, value);
        // send player scored event (in case everyone resets when someone scores)
    }

    // return a color based on input team
    public static Color GetColor(int team)
    {
        if (instance == null || instance.teams.Count == 0)
        {
            return Color.white;
        }
        return instance.teams[team].teamColor;
    }
}

[System.Serializable]
public class Team
{
    public Color teamColor;
    public Transform teamSpawn;
}