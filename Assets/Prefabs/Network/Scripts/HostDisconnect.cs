using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// for now, no host migration.  when the room creator leaves everyone else leaves also
/// this listens for the host to leave the room and fires an event (likely the same UI_LeaveRoom event to force player leaving)
/// </summary>

public class HostDisconnect : MonoBehaviourPunCallbacks
{
    [SerializeField] private ScriptableObjectArchitecture.GameEvent onHostDisconnectEvent = default;
    private Player host;

    /// <summary>
    /// Called when local player joins a photon room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        // store who the current host is
        host = PhotonNetwork.MasterClient;
    }

    /// <summary>
    /// Called when any player leaves photon room.
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // if the host leaves raise an event
        if (host != null && otherPlayer == host)
        {
            onHostDisconnectEvent.Raise();
        }
    }
}
