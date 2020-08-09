using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float despawnTimer = 5;
    [SerializeField] private float speed = 5;
    private Transform thisTrans;
    private GameObject thisObj;
    private Rigidbody rb;

    private void Awake()
    {
        thisTrans = transform;
        thisObj = gameObject;
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot()
    {
        rb.velocity = speed * thisTrans.forward;
        LeanPool.Despawn(thisObj, despawnTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: bullets still hit firing player sometimes, need to add that collider to an ignore list
        LeanPool.Despawn(thisObj);
    }
}