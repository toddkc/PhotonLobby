using Photon.Pun;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void TriggerScore()
    {
        Debug.LogError("player triggered score");
        if (!view.IsMine || !GameMode.instance.IsGameActive) return;
        GameMode.instance.PlayerScored(PhotonNetwork.LocalPlayer, 1);
    }
}