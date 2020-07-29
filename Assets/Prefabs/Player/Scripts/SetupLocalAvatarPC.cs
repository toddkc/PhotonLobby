using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a pc avatar will have various components that need to be disabled for non-local players
/// </summary>

public class SetupLocalAvatarPC : MonoBehaviour
{
    [SerializeField] private int localAvatarLayer = default;
    [SerializeField] private List<Transform> avatarObjects = new List<Transform>();
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
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

    public void SetPlayerColors()
    {
        var _team = CustomPlayerProperties.GetTeam(view.Owner);
        var _color = GameManager.GetColor(_team);
        foreach (Transform child in avatarObjects)
        {
            if (child.name != "Body") continue;
            var _renderer = child.GetComponent<Renderer>();
            var _block = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_block);
            _block.SetColor("_Color", _color);
            _renderer.SetPropertyBlock(_block);
        }
    }
}