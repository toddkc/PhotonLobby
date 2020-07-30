using UnityEngine;

public class DespawnPlayer : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("triggered");
        var spawner = collision.gameObject.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerRespawn(respawnDelay);
    }

    private void OnCollisionEnter(Collider other)
    {
        Debug.LogError("triggered");
        var spawner = other.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerRespawn(respawnDelay);
    }
}