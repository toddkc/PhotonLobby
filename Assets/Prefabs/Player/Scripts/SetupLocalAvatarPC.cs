using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// a pc avatar will have various components that need to be disabled for non-local players
/// </summary>

public class SetupLocalAvatarPC : MonoBehaviour
{
    [SerializeField] private int localAvatarLayer = default;
    [SerializeField] private List<Transform> avatarObjects = new List<Transform>();

    private void Start()
    {
        var view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            SetupLocal();
        }
        else
        {
            SetupNetwork();
        }
    }


    // adjust the avatar mesh layers so local player doesn't see their body
    private void SetupLocal()
    {
        foreach (Transform child in avatarObjects)
        {
            child.gameObject.layer = localAvatarLayer;
        }
    }

    // turn off components on network players
    private void SetupNetwork()
    {
        // character controller - disable
        GetComponent<PCLobbyController>().enabled = false;
        // camera controller = disable
        GetComponent<MouseCameraController>().enabled = false;
        // camera object - destroy
        Destroy(GetComponentInChildren<Camera>().gameObject);
        // canvas - destroy
        Destroy(GetComponentInChildren<Canvas>().gameObject);

    }
}