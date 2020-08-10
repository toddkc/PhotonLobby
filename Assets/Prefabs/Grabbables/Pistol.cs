using Photon.Pun;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private float fireRate = 1;
    [SerializeField] private Transform fireTrans = default;
    [SerializeField] private GameObject model = default;
    [SerializeField] private GameObject bulletPrefab = default;
    [SerializeField] private AudioClip shootClip = default;
    [SerializeField] private GameObject shootEffect = default;
    [SerializeField] private OVRInput.Controller holdingController = default;
    [SerializeField] private string linkedShootName = default;
    private PhotonView view;
    public bool isHeld;
    private float fireDelay;
    private PlayerShoot playerShoot;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
        playerShoot = GetComponentInParent<PlayerShoot>();
    }

    private void OnEnable()
    {
        fireDelay = Time.time + fireRate;
        isHeld = true;
    }

    private void OnDisable()
    {
        isHeld = false;
    }

    private void Update()
    {
        if (!view.IsMine) return;
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
            playerShoot.Shoot(fireTrans, bulletPrefab, shootClip, shootEffect);
        }
    }

}