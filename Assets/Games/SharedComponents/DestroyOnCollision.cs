using Lean.Pool;
using UnityEngine;

/// <summary>
/// Despawn an object when it collides with another object.
/// Use the physics matrix to determine what collides with what.
/// </summary>

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