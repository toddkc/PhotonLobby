using Photon.Pun;
using UnityEngine;

/// <summary>
/// The player controller and camera controller work with a parent/child setup
/// The parent moves but does not rotate, the child rotates
/// This will track the rotation while the parent TransformView tracks that object
/// </summary>

public class TrackNetworkRotation : MonoBehaviour, IPunObservable
{
    [SerializeField] private Transform objectToTrack = default;
    private Quaternion rotation;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rotation);
        }
        else
        {
            rotation = (Quaternion)stream.ReceiveNext();
            OnRotation();
        }
    }

    private void FixedUpdate()
    {
        if (!view.IsMine)
        {
            OnRotation();
            return;
        }
        rotation = objectToTrack.rotation;
    }

    private void OnRotation()
    {
        objectToTrack.rotation = rotation;
    }
}