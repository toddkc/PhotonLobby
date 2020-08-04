using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerRotatePC : PlayerRotateBase
{
    private CatlikeController characterController;
    private MouseCameraController cameraController;
    private PhotonView view;

    protected override void Awake()
    {
        thisTransform = transform;
        characterController = GetComponent<CatlikeController>();
        cameraController = GetComponentInChildren<MouseCameraController>();
        view = GetComponent<PhotonView>();
    }

    public override void Rotate(Quaternion rotation)
    {
        if (view != null && !view.IsMine) return;
        characterController.enabled = false;
        //cameraController.enabled = false;
        cameraController.Disable();
        thisTransform.rotation = rotation * thisTransform.rotation;
        StartCoroutine(ReenablePlayer());
    }

    private IEnumerator ReenablePlayer()
    {
        yield return new WaitForSeconds(0.4f);
        cameraController.ResetView();
        yield return null;
        //cameraController.enabled = true;
        yield return null;
        cameraController.ResetView();
        yield return null;
        characterController.enabled = true;
    }
}