using Lean.Pool;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    private GameObject gObj;

    private void Awake()
    {
        gObj = gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        LeanPool.Despawn(gObj);
    }

    private void OnTriggerEnter(Collider other)
    {
        LeanPool.Despawn(gObj);
    }
}