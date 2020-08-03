using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ObstacleCourseGame : GameMode
{
    private bool isGameActive = false;
    private PhotonView view;
    [SerializeField] private int maxScore = 3;

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

    private void ResetGame()
    {
        InputBridgeBase.ToggleMovement(false);
        CustomPlayerProperties.ResetProps(PhotonNetwork.LocalPlayer);
        isGameActive = false;
        if (PhotonNetwork.IsMasterClient)
        {
            CustomRoomProperties.InitializeRoom(PhotonNetwork.CurrentRoom, PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    public override void PlayerScored(Player player, int value)
    {
        Debug.LogError("player scored: " + value);
        if (!isGameActive) return;
        // add score to player
        CustomPlayerProperties.AddScore(player, value);
        // add score to player team
        var team = CustomPlayerProperties.GetTeam(player);
        CustomRoomProperties.AddScore(PhotonNetwork.CurrentRoom, team, value);
        if (!IsGameActive)
        {
            CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 3);
        }
    }

    public override bool IsGameActive
    {
        get
        {
            if (instance == null || !isGameActive || CustomRoomProperties.GetGameState(PhotonNetwork.CurrentRoom) != 2) return false;
            if (CheckScores() != -1)
            {
                Debug.LogError("isgameactive: false");
                return false;
            }
            Debug.LogError("isgameactive: true");
            return true;
        }
        set
        {
            Debug.LogError("set isgameactive: " + value);
            isGameActive = value;
        }
    }

    public override void SetupAndSpawn()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            int _teamIndex = Team.GetNextTeam();
            PhotonNetwork.CurrentRoom.AddToTeam(_teamIndex, 1);
            player.Value.SetTeam(_teamIndex);
            view.RPC("SpawnAvatar", player.Value, _teamIndex);
        }
    }

    public override int CheckScores()
    {
        int[] scores = PhotonNetwork.CurrentRoom.GetScores();
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= maxScore)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Spawn a player avatar on the network.
    /// </summary>
    [PunRPC]
    public override void SpawnAvatar(int teamIndex)
    {
        Vector3 _randomSpawn = teams[teamIndex].GetSpawn();
        Transform _startPos = teams[teamIndex].teamSpawn;
        var localAvatar = PhotonNetwork.Instantiate(avatar.name, _randomSpawn, _startPos.rotation);
    }
}