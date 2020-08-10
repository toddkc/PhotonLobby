using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
    private PhotonView view;

    [SerializeField]
    private List<GameObject> left = new List<GameObject>();
    [SerializeField]
    private List<GameObject> right = new List<GameObject>();

    public List<GameObject> leftHandPlayerItems = new List<GameObject>();
    public List<GameObject> rightHandPlayerItems = new List<GameObject>();

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void SwitchItem(int hand, int model)
    {
        if (!view.IsMine) return;
        if (hand == 0)
        {
            foreach (var item in leftHandPlayerItems)
            {
                item.SetActive(false);
            }
            foreach (var item in left)
            {
                item.SetActive(false);
            }
            if (model >= 0) leftHandPlayerItems[model].SetActive(true);
            if (model >= 0) left[model].SetActive(true);
        }
        else
        {
            foreach (var item in rightHandPlayerItems)
            {
                item.SetActive(false);
            }
            foreach (var item in right)
            {
                item.SetActive(false);
            }
            if (model >= 0) rightHandPlayerItems[model].SetActive(true);
            if (model >= 0) right[model].SetActive(true);
        }

        view.RPC("RPCItemSwitch", RpcTarget.Others, hand, model);
    }

    private void SwitchAvatarModel(int hand, int model)
    {
        if (view.IsMine) return;
        if (hand == 0)
        {
            foreach (var item in left)
            {
                item.SetActive(false);
            }
            if (model >= 0) left[model].SetActive(true);
        }
        else
        {
            foreach (var item in right)
            {
                item.SetActive(false);
            }
            if (model >= 0) right[model].SetActive(true);
        }
    }

    [PunRPC]
    private void RPCItemSwitch(int hand, int item)
    {
        SwitchAvatarModel(hand, item);
    }
}