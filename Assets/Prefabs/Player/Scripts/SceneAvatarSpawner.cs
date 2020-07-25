using Photon.Pun;
using UnityEngine;

public class SceneAvatarSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnImmediate = true;
    [SerializeField] private GameObject prefab = default;

    private void Start()
    {
        if (spawnImmediate)
        {
            SpawnAvatar(Vector3.zero, Quaternion.identity);
        }
        else
        {
            SetupSpawns();
        }
    }

    private void SpawnAvatar(Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return;
        PhotonNetwork.Instantiate(prefab.name, position, rotation);
    }

    private void SetupSpawns()
    {
        // this needs to use the room/player properties to determine where to spawn
    }
}
