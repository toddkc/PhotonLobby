using Photon.Pun;
using UnityEngine;

/// <summary>
/// this can be used to spawn a network avatar if not using a gamemanager to setup the scene
/// </summary>

public class SceneAvatarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab = default;

    private void Start()
    {
        SpawnAvatar(Vector3.zero, Quaternion.identity);
    }

    private void SpawnAvatar(Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;
        PhotonNetwork.Instantiate(prefab.name, position, rotation);
    }
}
