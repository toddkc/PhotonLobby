using UnityEngine;

public class PCLobbyController : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private KeyCode leftRotate = default;
    [SerializeField] private KeyCode rightRotate = default;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;

        movement.z += Input.GetAxis("Vertical");
        movement.x += Input.GetAxis("Horizontal");

        thisTransform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        var rotation = thisTransform.eulerAngles;

        if (Input.GetKeyDown(rightRotate))
        {
            rotation.y += 45;
        }
        if (Input.GetKeyDown(leftRotate))
        {
            rotation.y -= 45;
        }

        thisTransform.eulerAngles = rotation;
    }
}
