﻿using Photon.Pun;
using UnityEngine;

/// <summary>
/// this component is used to make an object follow a local PUN player.
/// useful for having network synced objects that track the local player across the network
/// </summary>

public class FollowLocalPlayer : MonoBehaviourPun
{
    [SerializeField] private float lerpSpeed = 10f;
    private bool followPlayer = false;
    private Transform playerToFollow;
    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
        }
        else
        {
            playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
            followPlayer = true;
        }
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (!followPlayer) return;
        // get location of player
        var target = playerToFollow.position;
        // get location of movement
        var movement = Vector3.Lerp(thisTransform.position, target, lerpSpeed * Time.deltaTime);
        // move to that location
        thisTransform.position = movement;
    }
}