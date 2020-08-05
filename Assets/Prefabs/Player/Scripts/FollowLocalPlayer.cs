using Photon.Pun;
using UnityEngine;

/// <summary>
/// this component is used to make an object follow a PUN player.
/// the actual photontransformview object may lag so this will always move towards the last known position
/// </summary>

public class FollowLocalPlayer : MonoBehaviourPun
{
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] Transform followingObject = default;
    [SerializeField] Transform objectToFollow = default;
    [SerializeField] Transform objectToRotateWith = default;

    private void LateUpdate()
    {
        if (objectToFollow == null) return;
        FollowPlayer();
        Rotate();
    }

    private void FollowPlayer()
    {
        var target = objectToFollow.position;
        var movement = Vector3.Lerp(followingObject.position, target, lerpSpeed * Time.deltaTime);
        followingObject.position = movement;
    }

    private void Rotate()
    {
        followingObject.rotation = objectToRotateWith.rotation;
    }
}
