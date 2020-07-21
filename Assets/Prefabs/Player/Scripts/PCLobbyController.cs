using UnityEngine;

/// <summary>
/// basic controller to move around the lobby scene
/// </summary>

public class PCLobbyController : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] private Transform forwardTransform = default;
    [SerializeField] private float moveSpeed = 5;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        if(forwardTransform == null || InputBridgePC.instance == null)
        {
            Debug.LogError("PC Lobby Controller not setup properly!", this);
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = Vector3.zero;

        movement += Vector3.ProjectOnPlane(
                forwardTransform.right,
                thisTransform.up)
                .normalized * InputBridgePC.instance.StrafeAxis;
        movement += Vector3.ProjectOnPlane(
            forwardTransform.forward,
            thisTransform.up)
            .normalized * InputBridgePC.instance.MoveAxis;

        thisTransform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
