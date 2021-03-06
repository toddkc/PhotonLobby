﻿using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerRotatePC : PlayerRotateBase
{
    [SerializeField] private float delayTime = 0.2f;
    private WaitForSeconds delay;
    private CatlikeController characterController;
    private MouseCameraController cameraController;
    private PhotonView view;

    protected override void Awake()
    {
        thisTransform = transform;
        characterController = GetComponent<CatlikeController>();
        cameraController = GetComponentInChildren<MouseCameraController>();
        view = GetComponent<PhotonView>();
        delay = new WaitForSeconds(delayTime);
    }

    public override void Rotate(Quaternion rotation)
    {
        if (view != null && !view.IsMine) return;
        characterController.enabled = false;
        cameraController.enabled = false;
        thisTransform.rotation = rotation * thisTransform.rotation;
        StartCoroutine(ReenablePlayer());
    }

    private IEnumerator ReenablePlayer()
    {
        yield return delay;
        cameraController.enabled = true;
        characterController.enabled = true;
    }
}