using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this component will hide any meshes so the local player won't see themselves
/// </summary>

public class SetupLocalAvatar : MonoBehaviourPun
{
    [SerializeField] private int localAvatarLayer = default;
    [SerializeField] private List<Transform> avatarObjects = new List<Transform>();

    private void Start()
    {
        if (!photonView.IsMine) return;
        foreach (Transform child in avatarObjects)
        {
            child.gameObject.layer = localAvatarLayer;
        }
    }
}
