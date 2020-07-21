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
        Vector3 movement = Vector3.zero;

        movement.z += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        movement.x += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;

        thisTransform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        var rotation = thisTransform.eulerAngles;

        if (OVRInput.GetDown(rightRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            rotation.y += 45;
        }
        if (OVRInput.GetDown(leftRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            rotation.y -= 45;
        }

        thisTransform.eulerAngles = rotation;
    }
}
