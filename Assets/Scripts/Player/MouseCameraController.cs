using UnityEngine;

public class MouseCameraController : MonoBehaviour
{
    [Range(0f, 90f)]
    [SerializeField]
    float upperVerticalLimit = 60f;
    [Range(0f, 90f)]
    [SerializeField]
    float lowerVerticalLimit = 60f;
    [SerializeField]
    float rotateSpeed = 10f;

    private Transform thisTransform;
    private Transform camTransform;

    private void Awake()
    {
        thisTransform = transform;
        camTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        Pitch();
        Yaw();
    }

    // move cam up/down
    private void Pitch()
    {
        float xAngle = camTransform.eulerAngles.x
    }

    // rotate gameobject
    private void Yaw()
    {

    }
}
