using Photon.Pun;
using UnityEngine;

/// <summary>
/// this component will hide any meshes so the local player won't see themselves
/// </summary>

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
