using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private bool randomize = false;
    [SerializeField] private float speed = 5;
    private Transform thisTransform;
    private Rigidbody rb;

    private void Awake()
    {
        thisTransform = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (randomize)
        {
            var _speed = Random.Range(speed - 0.25f, speed + 0.25f);
            speed = _speed;
        }
        speed = speed * 100;
    }

    private void FixedUpdate()
    {
        rb.velocity = thisTransform.forward * speed * Time.deltaTime;
    }
}