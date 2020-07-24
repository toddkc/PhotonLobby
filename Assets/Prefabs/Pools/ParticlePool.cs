using System.Collections;
using UnityEngine;

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

    public override void Despawn(GameObject instance)
    {
        StartCoroutine(DespawnDelay(instance));
        
    }

    private IEnumerator DespawnDelay(GameObject instance)
    {
        yield return delay;
        ActualDespawn(instance);
    }

    private void ActualDespawn(GameObject instance)
    {
        active.Remove(instance);
        inactive.Add(instance);
        instance.SetActive(false);
    }
}
