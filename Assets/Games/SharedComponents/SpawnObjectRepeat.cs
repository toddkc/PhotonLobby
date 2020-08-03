using Lean.Pool;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn an object at a repeating interval.
/// </summary>

public class SpawnObjectRepeat : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn = default;
    [SerializeField] private bool randomize = false;
    [SerializeField] private float spawnTime = 5;
    private Transform thisTransform;
    private WaitForSeconds spawnDelay;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        if (randomize)
        {
            var _speed = Random.Range(spawnTime - 1, spawnTime + 1);
            spawnDelay = new WaitForSeconds(_speed);
        }
        else
        {
            spawnDelay = new WaitForSeconds(spawnTime);
        }
        StartCoroutine(SpawnCountdown());

    }

    private void SpawnObject()
    {
        LeanPool.Spawn(objectToSpawn, thisTransform.position, thisTransform.rotation);
    }

    private IEnumerator SpawnCountdown()
    {
        while (true)
        {
            SpawnObject();
            yield return spawnDelay;
        }
    }
}