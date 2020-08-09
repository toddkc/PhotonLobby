using Lean.Pool;
using Photon.Pun;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PhotonView view;
    private GameObject currentBullet;
    private AudioClip currentAudio;
    private CatlikeController controller;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        controller = transform.root.GetComponentInChildren<CatlikeController>();
    }

    public void Shoot(Vector3 position, Quaternion rotation, GameObject shootobject, AudioClip shootaudio)
    {
        currentBullet = shootobject;
        currentAudio = shootaudio;

        // TODO: this is a work in progress
        // add current velocity to shoot 
        //var _momentum = controller.CurrentVelocity;
        //var _posupdate = position + _momentum;

        if (view.IsMine)
        {
            if (currentBullet)
            {
                var _bullet = LeanPool.Spawn(currentBullet, position, rotation);
                _bullet.GetComponent<Bullet>().Shoot();
            }
            if (currentAudio) AudioManager.instance.PlayClipAtSource(currentAudio, position);
        }
        else
        {
            view.RPC("CmdShoot", RpcTarget.Others, position, rotation);
        }
    }

    [PunRPC]
    private void CmdShoot(Vector3 position, Quaternion rotation)
    {
        if (currentBullet)
        {
            var _bullet = LeanPool.Spawn(currentBullet, position, rotation);
            _bullet.GetComponent<Bullet>().Shoot();
        }
        if (currentAudio) AudioManager.instance.PlayClipAtSource(currentAudio, position);
    }
}