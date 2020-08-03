using Photon.Pun;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string teamName;
    public Color teamColor;
    public Transform teamSpawn;

    public Vector3 GetSpawn()
    {
        Vector3 pos = teamSpawn.position;
        BoxCollider col = teamSpawn.GetComponent<BoxCollider>();

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

    public static int GetNextTeam()
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
}