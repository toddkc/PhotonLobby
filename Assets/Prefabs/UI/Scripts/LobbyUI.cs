using Photon.Pun;
using UnityEngine;

/// <summary>
/// this ui panel allows the host to select a game scene to load
/// </summary>

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject panel = default;
    [SerializeField] private GameEvent hostSelectGameEvent = default;

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            panel.SetActive(false);
        }
    }

    public void HostSelectGame(int index)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PlayerPrefs.SetInt("game", index);
        hostSelectGameEvent.Raise();
    }
}
