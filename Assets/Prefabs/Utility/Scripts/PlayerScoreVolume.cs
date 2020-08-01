using UnityEngine;

public class PlayerScoreVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var spawner = collision.gameObject.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerScore();
    }

    private void OnTriggerEnter(Collider other)
    {
        var spawner = other.GetComponent<PlayerRespawn>();
        if (spawner == null) return;
        spawner.TriggerScore();
    }
}