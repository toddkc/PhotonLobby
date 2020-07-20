using UnityEngine;

public class CameraRigMovement : MonoBehaviour
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
        MoveRig();
        RotateRig();
    }

    private void MoveRig()
    {
        Vector3 movement = Vector3.zero;

        movement.z += Input.GetAxis("Vertical");
        movement.z += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;

        movement.x += Input.GetAxis("Horizontal");
        movement.x += OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;

        thisTransform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void RotateRig()
    {
        var rotation = thisTransform.eulerAngles;
        if (Input.GetKeyDown(KeyCode.E) || OVRInput.GetDown(rightRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            rotation.y += 45;
        }
        if (Input.GetKeyDown(KeyCode.Q) || OVRInput.GetDown(leftRotate) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            rotation.y -= 45;
        }
        thisTransform.eulerAngles = rotation;
    }
}
