using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    private PhotonView view;

    [SerializeField]
    private List<Transform> left = new List<Transform>();
    [SerializeField]
    private List<Transform> right = new List<Transform>();

    public List<IGrabbable> leftHandPlayerItems = new List<IGrabbable>();
    public List<IGrabbable> rightHandPlayerItems = new List<IGrabbable>();
    private List<IGrabbable> leftHandAvatarItems = new List<IGrabbable>();
    private List<IGrabbable> rightHandAvatarItems = new List<IGrabbable>();

    // local player will switch hand model
    // send rpc to everyone else
    // rpc tells them to switch avatar model

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        foreach (Transform trans in left)
        {
            leftHandAvatarItems.Add(trans.GetComponent<IGrabbable>());
        }
        foreach (Transform trans in right)
        {
            rightHandAvatarItems.Add(trans.GetComponent<IGrabbable>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchItem(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchItem(1, 0);
        }
    }

    public void SwitchItem(int hand, int model)
    {
        if (!view.IsMine) return;
        if (hand == 0)
        {
            foreach (var item in leftHandPlayerItems)
            {
                item.OnDrop();
            }
            if (model >= 0) leftHandPlayerItems[model].OnGrab();
        }
        else
        {
            foreach (var item in rightHandPlayerItems)
            {
                item.OnDrop();
            }
            if (model >= 0) rightHandPlayerItems[model].OnGrab();
        }

        view.RPC("RPCItemSwitch", RpcTarget.Others, hand, model);
    }

    private void SwitchAvatarModel(int hand, int model)
    {
        if (view.IsMine) return;
        if (hand == 0)
        {
            foreach (var item in leftHandAvatarItems)
            {
                item.OnDrop();
            }
            if (model >= 0) leftHandAvatarItems[model].OnGrab();
        }
        else
        {
            foreach (var item in rightHandAvatarItems)
            {
                item.OnDrop();
            }
            if (model >= 0) rightHandAvatarItems[model].OnGrab();
        }
    }

    [PunRPC]
    private void RPCItemSwitch(int hand, int item)
    {
        SwitchAvatarModel(hand, item);
    }
}