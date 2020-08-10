using Lean.Pool;
using Photon.Pun;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PhotonView view;
    private GameObject currentBullet;
    private GameObject currentEffect;
    private AudioClip currentAudio;
    private CatlikeController controller;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        controller = transform.root.GetComponentInChildren<CatlikeController>();
    }

    public void Shoot(Transform shootpoint, GameObject shootobject, AudioClip shootaudio, GameObject shooteffect)
    {
        currentBullet = shootobject;
        currentAudio = shootaudio;
        currentEffect = shooteffect;

        // TODO: this is a work in progress
        // add current velocity to shoot 
        //var _momentum = controller.CurrentVelocity;
        //var _posupdate = position + _momentum;

        if (view.IsMine)
        {
            var _bullet = LeanPool.Spawn(currentBullet, shootpoint.position, shootpoint.rotation);
            _bullet.GetComponent<Bullet>().Shoot();
            var _effect = LeanPool.Spawn(shooteffect, shootpoint);
            _effect.GetComponent<DespawnEffect>().EnableEffect();
            AudioManager.instance.PlayClipAtSource(currentAudio, shootpoint.position);
        }
        else
        {
            view.RPC("CmdShoot", RpcTarget.Others, shootpoint.position, shootpoint.rotation);
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
        if (currentEffect) LeanPool.Spawn(currentEffect, position, rotation);
    }
}