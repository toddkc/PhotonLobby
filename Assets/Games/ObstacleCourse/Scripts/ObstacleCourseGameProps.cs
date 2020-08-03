using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ObstacleCourseGameProps : GameProps
{
    [SerializeField] private StringReference uiMessage = default;

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // this will update when the game state changes
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.game))
        {
            int newstate = (int)propertiesThatChanged[CustomRoomProperties.game];
            if (newstate == 1 && PhotonNetwork.IsMasterClient)
            {
                GameMode.instance.SetupAvatars();
                GameMode.instance.StartGame();
            }
            else if (newstate == 2)
            {
                InputBridgeBase.ToggleMovement(true);
                uiMessage.Value = "Start!";
            }
            else if (newstate == 3)
            {
                InputBridgeBase.ToggleMovement(false);
                GameMode.instance.EndGame();
            }
        }

        // this will update when a player scores
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.scores) && GameMode.instance.IsGameActive)
        {
            GameMode.instance.CheckGame();
        }

        // this will update when the interest in a team is updated
        // ie when a team wins
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.interest) && GameMode.instance.IsGameActive)
        {
            if (!GameMode.instance.IsGameActive) return;
            var index = (int)propertiesThatChanged[CustomRoomProperties.interest];
            if (index == -1) return;
            var team = GameMode.instance.Teams[index].teamName;
            uiMessage.Value = $"{team} won!";
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (GameMode.instance == null || !GameMode.instance.IsGameActive) return;
        // this will update when players score
        // display message that they scored
        var team = CustomPlayerProperties.GetTeam(targetPlayer);
        var teamname = GameMode.instance.Teams[team].teamName;
        uiMessage.Value = $"{targetPlayer.NickName} ({teamname}) scored!";
    }
}