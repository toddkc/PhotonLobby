using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public abstract class GameProps : MonoBehaviourPunCallbacks
{
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.LogError("room props base");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.LogError("player props base");
    }
}