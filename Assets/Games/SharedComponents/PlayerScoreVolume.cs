using UnityEngine;

/// <summary>
/// Trigger the player scoring on collision.
/// Use the physics matrix to determine what collides with what.
/// </summary>

public class PlayerScoreVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var score = collision.gameObject.GetComponent<PlayerScore>();
        if (score == null) return;
        score.TriggerScore();
    }

    private void OnTriggerEnter(Collider other)
    {
        var score = other.GetComponent<PlayerScore>();
        if (score == null) return;
        score.TriggerScore();
    }
}