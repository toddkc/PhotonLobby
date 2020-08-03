using Photon.Pun;
using Photon.Realtime;
using ScriptableObjectArchitecture;
using UnityEngine;

public class ObstacleCourseGameProps : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameEvent stopGameEvent = default;
    [SerializeField] private GameEvent startGameEvent = default;
    [SerializeField] private StringReference uiMessage = default;

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.LogError("room props");
        if (propertiesThatChanged.ContainsKey(CustomRoomProperties.game))
        {
            int newstate = (int)propertiesThatChanged[CustomRoomProperties.game];
            if (newstate == 1)
            {
                GameMode.instance.SetupAndSpawn();
                StartCoroutine(GameMode.instance.StartGameCountdown());
            }
            else if (newstate == 2)
            {
                startGameEvent.Raise();
                InputBridgeBase.ToggleMovement(true);
                uiMessage.Value = "Start!";
                GameMode.instance.IsGameActive = true;
            }
            else if (newstate == 3)
            {
                // TODO: this needs to be on room props
                // all players don't get this
                //if (teams.WinningTeam != -1)
                //{
                //    var index = teams.WinningTeam;
                //    uiMessage.Value = $"{teams.Teams[index].teamName} Team Won!";
                //}
                GameMode.instance.IsGameActive = false;
                InputBridgeBase.ToggleMovement(false);
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(GameMode.instance.ReturnToLobby());
                }
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.LogError("player props");
        if (changedProps.ContainsKey(CustomPlayerProperties.score) && (int)changedProps["score"] > 0)
        {
            var team = CustomPlayerProperties.GetTeam(targetPlayer);
            string teamName = GameMode.instance.teams[team].teamName;
            uiMessage.Value = $"{targetPlayer.NickName} ({teamName} Team) scored!";
        }
    }
}