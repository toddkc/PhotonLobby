using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui used to show player/team score in game scene
/// </summary>

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text scoreText = default;
    private Dictionary<int, List<string>> playerscores = new Dictionary<int, List<string>>();

    // photon callback, will update score
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomPlayerProperties.score))
        {
            UpdateScoreText();
        }
    }

    // loop over players and show their scores
    private void UpdateScoreText()
    {
        playerscores.Clear();
        string _scoretext = "";
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            var _score = CustomPlayerProperties.GetCurrentScore(player.Value);
            if (playerscores.ContainsKey(_score))
            {
                playerscores[_score].Add(player.Value.NickName);
            }
            else
            {
                playerscores.Add(_score, new List<string>(new string[] { player.Value.NickName }));
            }
        }
        var _keys = playerscores.Keys.ToList();
        _keys.Sort();

        var _counter = 1;
        for(var i = _keys.Count - 1; i >= 0; i--)
        {
            foreach(var pscore in playerscores[_keys[i]])
            {
                _scoretext += $"{_counter}. {pscore} : {_keys[i]} \n";
            }
        }
        scoreText.text = _scoretext;
    }
}