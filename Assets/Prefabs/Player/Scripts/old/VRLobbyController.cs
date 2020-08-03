using UnityEngine;

/// <summary>
/// basic controller for moving around the lobby scene
/// </summary>
/// 
public class VRLobbyController : MonoBehaviour
{
    private Transform thisTransform;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private OVRInput.RawButton leftRotate = default;
    [SerializeField] private OVRInput.RawButton rightRotate = default;

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
        // TODO:
        // controller, headset, or avatar based forward direction
        // would also need to update avatar rotation
        // (if you turn in real life, your avatar should also turn)

        //Vector3 movement = Vector3.zero;
        //movement.z += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        //movement.x += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        //thisTransform.Translate(movement * moveSpeed * Time.deltaTime);

        Vector3 _movement = Vector3.zero;
        Vector3 _velocity = Vector3.zero;
        _movement += thisTransform.right * InputBridgeBase.instance.StrafeAxis;
        _movement += thisTransform.forward * InputBridgeBase.instance.MoveAxis;
        _movement.Normalize();
        _velocity += _movement * moveSpeed;
        thisTransform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    private void Rotate()
    {
        var _rotation = thisTransform.eulerAngles;

        if (OVRInput.GetDown(rightRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            _rotation.y += 45;
        }
        if (OVRInput.GetDown(leftRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            _rotation.y -= 45;
        }

        thisTransform.eulerAngles = _rotation;
    }
}
