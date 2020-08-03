using UnityEngine;

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