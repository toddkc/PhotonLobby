using Photon.Pun;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private PhotonView view;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
    }

    public void TryGrab()
    {

    }

    [PunRPC]
    public void RPCTryGrabObject()
    {

    }
}