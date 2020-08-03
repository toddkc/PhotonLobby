using Photon.Pun;
using UnityEngine;

public class GameScoring : MonoBehaviour
{
    [SerializeField] private int maxScore = default;
    public void PlayerScored()
    {
        var player = PhotonNetwork.LocalPlayer;
        var score = 1;
        var team = CustomPlayerProperties.GetTeam(player);
        CustomPlayerProperties.AddScore(player, score);
        CustomRoomProperties.AddScore(PhotonNetwork.CurrentRoom, team, score);
    }

    public int CheckScores()
    {
        int[] scores = PhotonNetwork.CurrentRoom.GetScores();
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= maxScore)
            {
                GameMode.instance.EndGame();
                return i;
            }
        }
        return -1;
    }
}