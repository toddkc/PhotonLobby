using System.Collections;
using UnityEngine;

/// <summary>
/// special object pool that waits for a time to despawn
/// used to allow fx to play before despawning
/// </summary>

public class ParticlePool : ObjectPool
{
    [Header("Particle Pool Settings")]
    [SerializeField] private float despawnDelay = 2.0f;

    private WaitForSeconds delay;

    protected override void Awake()
    {
        base.Awake();
        delay = new WaitForSeconds(despawnDelay);
    }

    // despawn after a delay
    public override void Despawn(GameObject instance)
    {
        StartCoroutine(DespawnDelay(instance));
        
    }

    // the delay
    private IEnumerator DespawnDelay(GameObject instance)
    {
        yield return delay;
        ActualDespawn(instance);
    }

    // actually despawn the object
    private void ActualDespawn(GameObject instance)
    {
        active.Remove(instance);
        inactive.Add(instance);
        instance.SetActive(false);
    }
}
