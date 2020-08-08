using Photon.Pun;
using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;

public class SetupLocalAvatarVR : MonoBehaviour
{
    [SerializeField] private int localAvatarLayer = default;
    [SerializeField] private List<Transform> avatarObjects = new List<Transform>();
    [SerializeField] private List<GameEventListener> listeners = new List<GameEventListener>();
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
        var _catcontroller = GetComponentInChildren<CatlikeController>();
        var _camcontroller = GetComponentInChildren<VRCameraController>();
        var _camrig = GetComponentInChildren<OVRCameraRig>().gameObject;
        
        Destroy(_catcontroller);
        Destroy(_camcontroller);
        Destroy(_camrig);

        foreach(var listener in listeners)
        {
            listener.enabled = false;
        }

        //GetComponentInChildren<CatlikeController>().enabled = false;
        //GetComponentInChildren<VRCameraController>().enabled = false;
        //Destroy(GetComponentInChildren<OVRCameraRig>().gameObject);
    }

    public void SetPlayerColors()
    {
        // TODO:
        // not sure what to do here now that Space Robot Kyle is being used...
        //var _team = CustomPlayerProperties.GetTeam(view.Owner);
        //var _color = GameManager.GetColor(_team);
        //var _color = GameMode.instance.Teams[_team].teamColor;
        //foreach (Transform child in avatarObjects)
        //{
        //    if (child.name != "Body") continue;
        //    var _renderer = child.GetComponent<Renderer>();
        //    var _block = new MaterialPropertyBlock();
        //    _renderer.GetPropertyBlock(_block);
        //    _block.SetColor("_Color", _color);
        //    _renderer.SetPropertyBlock(_block);
        //}
    }

}