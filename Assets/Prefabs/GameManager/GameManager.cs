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
    #region variables
    public static GameManager instance;
    [SerializeField] protected int maxScore = 3;
    [SerializeField] protected float respawnTime = 5;
    [SerializeField] protected float lobbyDelay = 5;
    [SerializeField] protected float startDelay = 3;
    [SerializeField] protected GameObject avatar = default;
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent startGameEvent = default;
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent stopGameEvent = default;
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent displayMessageEvent = default;
    [SerializeField] protected List<Team> teams = new List<Team>();
    protected int winningTeam = -1;
    protected bool isGameActive = false;
    protected PhotonView view;
    // what state is our game in
    protected enum GameState
    {
        initializing = 0,
        pregame = 1,
        running = 2,
        postgame = 3
    }
    #endregion

    #region static
    /// <summary>
    /// Get the team color.
    /// </summary>
    public static Color GetColor(int team)
    {
        return instance.teams[team].teamColor;
    }

    /// <summary>
    /// Get a random spawnpoint within a designated team spawn area.
    /// </summary>
    public static Vector3 GetSpawn(int team)
    {
        Vector3 pos = instance.teams[team].teamSpawn.position;
        BoxCollider col = instance.teams[team].teamSpawn.GetComponent<BoxCollider>();

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

    /// <summary>
    /// Update a players score, and their teams score.
    /// </summary>
    public static void PlayerScored(Player player, int value)
    {
        if (!PhotonNetwork.IsMasterClient || instance == null || !instance.isGameActive) return;
        // add score to player
        CustomPlayerProperties.AddScore(player, value);
        // add score to player team
        var team = CustomPlayerProperties.GetTeam(player);
        CustomRoomProperties.AddScore(PhotonNetwork.CurrentRoom, team, value);

        Debug.Log("is game still active: " + instance.IsGameActive);

        if (!instance.IsGameActive)
        {
            CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 3);
        }
    }
    #endregion

    #region private
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        view = gameObject.AddComponent<PhotonView>();
        view.ViewID = 999;
    }

    protected virtual void Start()
    {
        ResetGame();
    }

    /// <summary>
    /// Reset the player props and room props.
    /// </summary>
    protected virtual void ResetGame()
    {
        InputBridgeBase.ToggleMovement(false);
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        winningTeam = -1;
        isGameActive = false;
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    /// <summary>
    /// Get the index of the next team to add a player to.
    /// </summary>
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

    /// <summary>
    /// Assign teams and spawn player avatar
    /// </summary>
    protected virtual void SetupTeams()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            int _teamIndex = GetNextTeam();
            PhotonNetwork.CurrentRoom.AddToTeam(_teamIndex, 1);
            player.Value.SetTeam(_teamIndex);
            view.RPC("SpawnAvatar", player.Value, _teamIndex);
        }
        StartCoroutine(StartGameCountdown());
    }

    /// <summary>
    /// Check the scores and determine if game is active or not
    /// </summary>
    protected virtual bool IsGameActive
    {
        get
        {
            if (instance == null || !instance.isGameActive || CustomRoomProperties.GetGameState(PhotonNetwork.CurrentRoom) != 2) return false;
            instance.CheckScores();
            if (instance.winningTeam != -1)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Check scores and assign a winning team if applicable.
    /// </summary>
    protected virtual void CheckScores()
    {
        if (winningTeam != -1) return;
        int[] scores = PhotonNetwork.CurrentRoom.GetScores();
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= maxScore)
            {
                winningTeam = i;
                break;
            }
        }
    }
    #endregion

    #region callbacks
    /// <summary>
    /// Callback used to monitor changes in the game state.
    /// </summary>
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("custom room props updated: " + propertiesThatChanged.ToStringFull());
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.game))
        {
            int newstate = (int)propertiesThatChanged[CustomRoomProperties.game];
            if (newstate == 1)
            {
                Debug.LogError("setup teams");
                SetupTeams();
            }
            else if (newstate == 2)
            {
                Debug.LogError("game start");
                startGameEvent.Raise();
                InputBridgeBase.ToggleMovement(true);
                PlayerPrefs.SetString("message", "Start!");
                displayMessageEvent.Raise();
                isGameActive = true;
            }
            else if (newstate == 3)
            {
                Debug.LogError("game end");
                if(winningTeam != -1)
                {
                    string message = "";
                    // TODO: if only one player on team say Player Won! rather than Team Won!
                    //if(CustomRoomProperties.GetTeams(PhotonNetwork.CurrentRoom)[winningTeam] == 1)
                    //{
                    //    var team = CustomPlayerProperties.GetTeam();
                    //    message = $"{} won!";
                    //}
                    //else
                    //{
                    //    message = $"{teams[winningTeam].teamName} won!";
                    //}
                    message = $"{teams[winningTeam].teamName} won!";
                    PlayerPrefs.SetString("message", message);
                    displayMessageEvent.Raise();
                }
                isGameActive = false;
                InputBridgeBase.ToggleMovement(false);
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(ReturnToLobby());
                }
            }
        }
    }

    /// <summary>
    /// Callback used to monitor changes to the player.
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("custom player props updated: " + changedProps.ToStringFull());
        if (changedProps.ContainsKey(CustomPlayerProperties.score) && (int)changedProps["score"] > 0)
        {
            string message = $"{targetPlayer.NickName} scored!";
            PlayerPrefs.SetString("message", message);
            displayMessageEvent.Raise();
        }
    }
    #endregion

    #region ienumerators
    /// <summary>
    /// Return to lobby after game ends, after a short delay
    /// </summary>
    protected virtual IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(lobbyDelay);
        stopGameEvent.Raise();
    }

    /// <summary>
    /// Host will start game after a delay
    /// </summary>
    private IEnumerator StartGameCountdown()
    {
        yield return new WaitForSeconds(startDelay);
        CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 2);
    }
    #endregion

    #region rpcs
    /// <summary>
    /// Spawn a player avatar on the network.
    /// </summary>
    [PunRPC]
    protected virtual void SpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = GetSpawn(teamIndex);
        Transform _startPos = teams[teamIndex].teamSpawn;
        PhotonNetwork.Instantiate(avatar.name, _randomSpawn, _startPos.rotation);
    }
    #endregion

















}

