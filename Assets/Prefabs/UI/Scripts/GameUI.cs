using Photon.Pun;
using UnityEngine;

/// <summary>
/// this will show a different menu for host/guest in game scene
/// guest should always be able to leave the ROOM and go back to the main menu
/// only host can load/unload the game scene
/// </summary>

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject hostPanel = default;
    [SerializeField] private GameObject guestPanel = default;

    private void Start()
    {
        bool isHost = PhotonNetwork.IsMasterClient;
        hostPanel.SetActive(isHost);
        guestPanel.SetActive(!isHost);
    }
}
