using UnityEngine;

/// <summary>
/// Cause the player to despawn on collision.
/// Use the physics matrix to determine what collides with what.
/// </summary>

public class DespawnPlayer : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        var spawner = collision.gameObject.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerRespawn(respawnDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        var spawner = other.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerRespawn(respawnDelay);
    }
}