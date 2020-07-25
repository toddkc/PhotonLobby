using UnityEngine;

/// <summary>
/// basic controller to move around the lobby scene
/// </summary>

public class PCLobbyController : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] private float moveSpeed = 5;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        if(InputBridgeBase.instance == null)
        {
            Debug.LogError("PC Lobby Controller cannot find input bridge!", this);
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 _movement = Vector3.zero;
        Vector3 _velocity = Vector3.zero;
        _movement += thisTransform.right * InputBridgeBase.instance.StrafeAxis;
        _movement += thisTransform.forward * InputBridgeBase.instance.MoveAxis;
        _movement.Normalize();
        _velocity += _movement * moveSpeed;
        thisTransform.Translate(_velocity * Time.deltaTime, Space.World);
    }
}
