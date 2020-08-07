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
        view = GetComponentInChildren<PhotonView>();
        if (view.IsMine)
        {
            SetupLocal();
        }
        else
        {
            SetupNetwork();
        }
    }


    /// <summary>
    /// Adjust the avatar mesh layers so local player doesn't see their body.
    /// </summary>
    private void SetupLocal()
    {
        foreach (Transform child in avatarObjects)
        {
            child.gameObject.layer = localAvatarLayer;
        }
    }

    /// <summary>
    /// Turn off certain components for networked players.
    /// </summary>
    private void SetupNetwork()
    {
        GetComponentInChildren<CatlikeController>().enabled = false;
        GetComponentInChildren<MouseCameraController>().enabled = false;
        Destroy(GetComponentInChildren<Camera>().gameObject);
        Destroy(GetComponentInChildren<Canvas>().gameObject);
    }

    /// <summary>
    /// Set the color of the player avatar based on team.
    /// </summary>
    public void SetPlayerColors()
    {
        var _team = CustomPlayerProperties.GetTeam(view.Owner);
        var _color = GameMode.instance.Teams[_team].teamColor;
        foreach (Transform child in avatarObjects)
        {
            if (child.name != "Team") continue;
            child.gameObject.SetActive(true);
            var _renderer = child.GetComponent<Renderer>();
            var _block = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_block);
            _block.SetColor("_Color", _color);
            _renderer.SetPropertyBlock(_block);
        }
    }
}