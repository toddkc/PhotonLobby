﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui used to show player/team score in game scene
/// </summary>

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text scoreText = default;
    private SortedList<int, string> playerscored = new SortedList<int, string>();

    // photon callback, will update score
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(CustomPlayerProperties.score))
        {
            Debug.LogError("update score!");
            UpdateScoreText();
        }
    }

    // loop over players and show their scores
    // TODO: this needs to work on the player or team level
    private void UpdateScoreText()
    {
        playerscored.Clear();
        string _scoretext = "";
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            playerscored.Add(CustomPlayerProperties.GetCurrentScore(player.Value), player.Value.NickName);
        }

        var _counter = 1;
        for(var i = playerscored.Count - 1; i >= 0; i--)
        {
            _scoretext += $"{_counter}.  {playerscored.Values[i]} : {playerscored.Keys[i]} \n ";
        }

        scoreText.text = _scoretext;
    }
}