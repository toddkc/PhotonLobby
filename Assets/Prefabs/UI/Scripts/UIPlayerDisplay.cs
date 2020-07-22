using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui display to show the current players in the room
/// </summary>

public class UIPlayerDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text text;

    private void Start()
    {
        text.text = "not in a room...";
        if (PhotonNetwork.InRoom)
        {
            UpdatePlayers();
        }
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayers();
    }

    public override void OnLeftRoom()
    {
        text.text = "not in a room...";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
    }

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
