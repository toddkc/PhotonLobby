﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// for now, no host migration.  when the room creator leaves everyone else leaves also
/// this listens for the host to leave the room and fires an event (likely the same UI_LeaveRoom event to force player leaving)
/// </summary>

public class HostDisconnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameEvent onHostDisconnectEvent = default;
    private Player host;

    public override void OnJoinedRoom()
    {
        host = PhotonNetwork.MasterClient;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (host != null && otherPlayer == host)
        {
            onHostDisconnectEvent.Raise();
        }
    }
}
