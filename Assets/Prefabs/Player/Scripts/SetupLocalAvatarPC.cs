using Photon.Pun;
using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a pc avatar will have various components that need to be disabled for non-local players
/// </summary>

public class SetupLocalAvatarPC : MonoBehaviour
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
        foreach (Transform child in avatarObjects)
        {
            Destroy(child.gameObject);
        }
    }

    // turn off components on network players
    private void SetupNetwork()
    {
        var _catcontroller = GetComponentInChildren<CatlikeController>();
        var _camcontroller = GetComponentInChildren<MouseCameraController>();

        Destroy(_catcontroller);
        Destroy(_camcontroller);

        foreach (var listener in listeners)
        {
            listener.enabled = false;
        }
    }
}