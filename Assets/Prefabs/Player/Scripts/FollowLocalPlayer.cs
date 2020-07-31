using Photon.Pun;
using UnityEngine;

/// <summary>
/// this component is used to make an object follow a PUN player.
/// the actual photontransformview object may lag so this will always move towards the last known position
/// </summary>

public class FollowLocalPlayer : MonoBehaviourPun
{
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] Transform playerToFollow;
    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void LateUpdate()
    {
        if (playerToFollow == null) return;
        FollowPlayer();
        Rotate();
    }

    private void FollowPlayer()
    {
        // get location of player
        var target = playerToFollow.position;
        // get location of movement
        var movement = Vector3.Lerp(thisTransform.position, target, lerpSpeed * Time.deltaTime);
        // move to that location
        thisTransform.position = movement;
    }

    private void Rotate()
    {
        thisTransform.localRotation = playerToFollow.localRotation;
    }
}
