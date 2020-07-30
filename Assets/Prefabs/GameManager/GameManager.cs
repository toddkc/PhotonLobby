using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

/// <summary>
/// this handles the logic for gameplay - it's a mess at the moment while I figure stuff out...
/// </summary>

[RequireComponent(typeof(TeamManager))]
public class GameManager : MonoBehaviourPunCallbacks
{
    #region variables
    public static GameManager instance;
    [Header("Avatar Prefab")]
    [SerializeField] protected GameObject avatar = default;
    [Header("Settings")]
    [SerializeField] protected float respawnTime = 5;
    [SerializeField] protected int maxScore = 3;
    [SerializeField] protected float lobbyDelay = 5;
    [SerializeField] protected float startDelay = 3;
    [Header("Events")]
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent startGameEvent = default;
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent stopGameEvent = default;
    [SerializeField] protected ScriptableObjectArchitecture.GameEvent displayMessageEvent = default;
    
    protected bool isGameActive = false;
    protected PhotonView view;
    private TeamManager teams;

    protected enum GameState
    {
        initializing = 0,
        pregame = 1,
        running = 2,
        postgame = 3
    }
    #endregion

    #region static (get color, player scored)

    /// <summary>
    /// Get the color to use for player avatar.
    /// </summary>
    public static Color GetColor(int team)
    {
        if(instance.teams == null)
        {
            return Color.white;
        }
        return instance.teams.GetColor(team);
    }

    /// <summary>
    /// Update a players score, and their teams score.
    /// </summary>
    public static void PlayerScored(Player player, int value)
    {
        if (!PhotonNetwork.IsMasterClient || !instance.isGameActive) return;
        instance.teams.PlayerScored(player, value);
        if (!instance.IsGameActive)
        {
            CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 3);
        }
    }
    #endregion

    #region private (reset, isgameactive, checkscores)
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        teams = GetComponent<TeamManager>();
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
        teams.WinningTeam = -1;
        isGameActive = false;
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
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
            if (teams.WinningTeam != -1)
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
        if (teams.WinningTeam != -1) return;
        int[] scores = PhotonNetwork.CurrentRoom.GetScores();
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= maxScore)
            {
                teams.WinningTeam = i;
                break;
            }
        }
    }
    #endregion

    #region callbacks (player props, room props)
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
                teams.SetupTeams();
                StartCoroutine(StartGameCountdown());
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
                if(teams.WinningTeam != -1)
                {
                    var index = teams.WinningTeam;
                    string message = $"{teams.Teams[index].teamName} Team Won!";
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
            var team = CustomPlayerProperties.GetTeam(targetPlayer);
            string teamName = teams.Teams[team].teamName;
            string message = $"{targetPlayer.NickName} ({teamName} Team) scored!";
            PlayerPrefs.SetString("message", message);
            displayMessageEvent.Raise();
        }
    }
    #endregion

    #region ienumerators (return to lobby, start game)
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

    #region rpcs (spawn avatar)
    /// <summary>
    /// Spawn a player avatar on the network.
    /// </summary>
    [PunRPC]
    protected virtual void SpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = teams.GetSpawn(teamIndex);
        Transform _startPos = teams.Teams[teamIndex].teamSpawn;
        PhotonNetwork.Instantiate(avatar.name, _randomSpawn, _startPos.rotation);
    }
    #endregion

}

