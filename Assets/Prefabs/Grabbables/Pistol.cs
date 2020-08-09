using Lean.Pool;
using Photon.Pun;
using UnityEngine;

public class Pistol : MonoBehaviour, IGrabbable
{
    [SerializeField] private float fireRate = 1;
    [SerializeField] private Transform fireTrans = default;
    [SerializeField] private GameObject model = default;
    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private AudioClip shootClip = default;
    [SerializeField] private OVRInput.Controller holdingController = default;
    private PhotonView view;
    private bool isHeld = false;
    private float fireDelay;
    private PlayerShoot playerShoot;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
        playerShoot = GetComponentInParent<PlayerShoot>();
    }

    private void Update()
    {
        if(isHeld && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, holdingController))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Time.time > fireDelay)
        {
            fireDelay = Time.time + fireRate;
            var _position = fireTrans.position;
            var _rotation = fireTrans.rotation;
            playerShoot.Shoot(_position, _rotation, bulletPrefab, shootClip);
        }
    }

    public void OnGrab()
    {
        model.SetActive(true);
        isHeld = true;
        fireDelay = Time.time + fireRate;
    }

    public void OnDrop()
    {
        model.SetActive(false);
        isHeld = false;
    }
}