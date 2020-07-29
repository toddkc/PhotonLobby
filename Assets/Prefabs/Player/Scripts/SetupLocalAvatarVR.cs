using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class SetupLocalAvatarVR : MonoBehaviour
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
        GetComponent<VRLobbyController>().enabled = false;
        // camerarig object - destroy
        Destroy(GetComponentInChildren<OVRCameraRig>().gameObject);
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