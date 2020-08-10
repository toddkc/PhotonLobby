using Lean.Pool;
using UnityEngine;

public class DespawnEffect : MonoBehaviour
{
    [SerializeField] private float delay;
    private GameObject thisObj;

    private void Awake()
    {
        thisObj = gameObject;
    }

    public void EnableEffect()
    {
        LeanPool.Despawn(thisObj, delay);
    }
}