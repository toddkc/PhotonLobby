using Photon.Pun;
using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;

/// <summary>
/// Logic for handling respawning of players.
/// Goes on the player.
/// Can be triggered by colliding with an object with a DespawnPlayer component.
/// </summary>

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject avatar = default;
    [SerializeField] private GameEvent respawnEvent = default;
    private PhotonView view;
    private Transform thisTransform;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        thisTransform = transform;
    }

    public void TriggerRespawn(float timer)
    {
        //if (!view.IsMine || !GameManager.IsGameActive) return;
        if (!view.IsMine || !GameMode.instance.IsGameActive) return;
        respawnEvent.Raise();
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
            //StartCoroutine(EnableAvatar());
            avatar.SetActive(true);
        }
        else
        {
            InputBridgeBase.ToggleMovement(true);
        }
    }

    private IEnumerator EnableAvatar()
    {
        //thisObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        avatar.SetActive(true);
    }

    [PunRPC]
    private void RPCDespawn()
    {
        if (!view.IsMine)
        {
            avatar.SetActive(false);
            thisTransform.localPosition = Vector3.zero;
            //thisObject.SetActive(false);
        }
        else
        {
            // TODO:
            // reset gravity when respawning, this doesn't work
            var _team = PhotonNetwork.LocalPlayer.GetTeam();
            var _spawn = GameMode.instance.Teams[_team].teamSpawn;
            thisTransform.rotation = _spawn.rotation;
            CustomGravity.UnregisterAll();

            InputBridgeBase.ToggleMovement(false);
            thisTransform.localPosition = Vector3.zero;
            respawnEvent.Raise();
        }
    }

    private void CmdRespawn()
    {
        view.RPC("RPCRespawn", RpcTarget.AllViaServer);
    }
}