using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private PhotonView view;
    private GameObject thisObject;
    private Transform thisTransform;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        thisObject = gameObject;
        thisTransform = transform;
    }

    public void TriggerRespawn(float timer)
    {
        if (!view.IsMine || !GameManager.IsGameActive) return;
        view.RPC("RPCDespawn", RpcTarget.All);
        StartCoroutine(RespawnTimer(timer));
    }

    private IEnumerator RespawnTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        CmdRespawn();
    }

    [PunRPC]
    private void RPCRespawn()
    {
        if (!view.IsMine)
        {
            thisObject.SetActive(true);
        }
        else
        {
            InputBridgeBase.ToggleMovement(true);
        }
    }

    [PunRPC]
    private void RPCDespawn()
    {
        if (!view.IsMine)
        {
            thisObject.SetActive(false);
        }
        else
        {
            InputBridgeBase.ToggleMovement(false);
            thisTransform.position = GameManager.GetSpawn(view.GetTeam());
        }
    }

    private void CmdRespawn()
    {
        view.RPC("RPCRespawn", RpcTarget.AllViaServer);
    }
}