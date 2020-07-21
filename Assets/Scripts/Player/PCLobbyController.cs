using UnityEngine;

public class PCLobbyController : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] private Transform forwardTransform;
    [SerializeField] private float moveSpeed = 5;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;

        if (forwardTransform == null)
        {
            movement += thisTransform.right * Input.GetAxis("Horizontal");
            movement += thisTransform.forward * Input.GetAxis("Vertical");
        }
        else
        {
            movement += Vector3.ProjectOnPlane(
                forwardTransform.right, 
                thisTransform.up)
                .normalized * Input.GetAxis("Horizontal");
            movement += Vector3.ProjectOnPlane(
                forwardTransform.forward, 
                thisTransform.up)
                .normalized * Input.GetAxis("Vertical");
        }

        thisTransform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
