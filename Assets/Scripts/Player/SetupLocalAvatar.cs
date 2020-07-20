using Photon.Pun;
using UnityEngine;

public class SetupLocalAvatar : MonoBehaviourPun
{
    [SerializeField] private int localAvatarLayer = default;

    private void Start()
    {
        if (!photonView.IsMine) return;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = localAvatarLayer;
        }
    }
}
