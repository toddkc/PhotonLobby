using Lean.Pool;
using Photon.Pun;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PhotonView view;
    private GameObject currentBullet;
    private AudioClip currentAudio;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void Shoot(Vector3 position, Quaternion rotation, GameObject shootobject, AudioClip shootaudio)
    {
        currentBullet = shootobject;
        currentAudio = shootaudio;
        view.RPC("CmdShoot", RpcTarget.AllViaServer, position, rotation);
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
        currentBullet = null;
        currentAudio = null;
    }
}