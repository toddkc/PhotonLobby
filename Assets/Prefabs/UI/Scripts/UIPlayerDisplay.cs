using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui display to show the current players in the room
/// </summary>

public class UIPlayerDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text text = default;

    private void Start()
    {
        text.text = "not in a room...";
        if (PhotonNetwork.InRoom)
        {
            UpdatePlayers();
        }
    }

    // photon callback
    public override void OnJoinedRoom()
    {
        UpdatePlayers();
    }

    // photon callback
    public override void OnLeftRoom()
    {
        text.text = "not in a room...";
    }

    // photon callback
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    // photon callback
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
    }

    // this will show all players in a room, who is the local, and who is the host
    // mainly for debug purposes
    private void UpdatePlayers()
    {
        string textstring = "";
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            textstring += player.Value.NickName;
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                textstring += "*";
            }
            if(player.Value == PhotonNetwork.MasterClient)
            {
                textstring += " - HOST";
            }
            textstring += "\n";
        }
        text.text = textstring;
    }
}
