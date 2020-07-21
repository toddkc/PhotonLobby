using Photon.Pun;
using UnityEngine;

public class SceneAvatarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab = default;
    private GameObject currentAvatar;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;
        currentAvatar = PhotonNetwork.Instantiate(prefab.name, Vector3.zero, Quaternion.identity);
    }
}
