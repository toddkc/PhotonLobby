using Photon.Pun;
using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;

public class SetupLocalAvatarVR : MonoBehaviour
{
    [SerializeField] private GameObject cameraRigPrefab = default;
    [SerializeField] private Transform cameraRigParent = default;
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
    // create camerarig for local player only
    private void SetupLocal()
    {
        Instantiate(cameraRigPrefab, cameraRigParent);
        var _centereye = Camera.main.transform;
        GetComponentInChildren<CatlikeController>().playerInputSpace = _centereye;
        foreach (Transform child in avatarObjects)
        {
            //child.gameObject.layer = localAvatarLayer;
            //Destroy(child.gameObject);
            SetChildrenLayers(child);
        }
    }

    private void SetChildrenLayers(Transform parent)
    {
        parent.gameObject.layer = localAvatarLayer;
        foreach(Transform child in parent)
        {
            child.gameObject.layer = localAvatarLayer;
            SetChildrenLayers(child);
        }
    }

    // turn off components on network players
    private void SetupNetwork()
    {
        var _catcontroller = GetComponentInChildren<CatlikeController>();
        var _camcontroller = GetComponentInChildren<VRCameraController>();
        
        Destroy(_catcontroller);
        Destroy(_camcontroller);

        foreach(var listener in listeners)
        {
            listener.enabled = false;
        }
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