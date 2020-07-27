using UnityEngine;

public class CharacterController_PC : MonoBehaviour
{
    private Transform thisTransform;
    private CharacterController controller;
    [SerializeField] private float moveSpeed = 5;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        thisTransform = transform;
    }

    private void Start()
    {
        if (InputBridgeBase.instance == null)
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
        //thisTransform.Translate(_velocity * Time.deltaTime, Space.World);

        //Vector3 _movement = Vector3.zero;
        //Vector3 _forward = thisTransform.TransformDirection(Vector3.forward);
        //Vector3 _right = thisTransform.TransformDirection(Vector3.right);
        //float _moveSpeed = moveSpeed * InputBridgeBase.instance.MoveAxis;
        //float _strafeSpeed = moveSpeed * InputBridgeBase.instance.StrafeAxis;
        //_movement += _forward * _moveSpeed;
        //_movement += _right * _strafeSpeed;
        controller.SimpleMove(_velocity);
    }
}