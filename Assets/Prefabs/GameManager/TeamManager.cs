using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class TeamManager : MonoBehaviour
{
    [SerializeField] private List<Team> teams = new List<Team>();
    public int WinningTeam { get; set; }
    public List<Team> Teams { get { return teams; } }
    private GameManager game;

    private void Awake()
    {
        game = GetComponent<GameManager>();
    }

    /// <summary>
    /// Get the team color.
    /// </summary>
    public Color GetColor(int team)
    {
        return teams[team].teamColor;
    }

    /// <summary>
    /// Get a random spawnpoint within a designated team spawn area.
    /// </summary>
    public Vector3 GetSpawn(int team)
    {
        Vector3 pos = teams[team].teamSpawn.position;
        BoxCollider col = teams[team].teamSpawn.GetComponent<BoxCollider>();

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
    /// Get the index of the next team to add a player to.
    /// </summary>
    public int GetNextTeam()
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
    public void SetupTeams()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            int _teamIndex = GetNextTeam();
            PhotonNetwork.CurrentRoom.AddToTeam(_teamIndex, 1);
            player.Value.SetTeam(_teamIndex);
            game.photonView.RPC("SpawnAvatar", player.Value, _teamIndex);
        }
    }

    /// <summary>
    /// Update a players score, and their teams score.
    /// </summary>
    public void PlayerScored(Player player, int value)
    {
        // add score to player
        CustomPlayerProperties.AddScore(player, value);
        // add score to player team
        var team = CustomPlayerProperties.GetTeam(player);
        CustomRoomProperties.AddScore(PhotonNetwork.CurrentRoom, team, value);
    }
}

[System.Serializable]
public class Team
{
    public string teamName;
    public Color teamColor;
    public Transform teamSpawn;
}