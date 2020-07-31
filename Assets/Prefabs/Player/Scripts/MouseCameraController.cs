using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// controller for the pc player to look around
/// </summary>

public class MouseCameraController : MonoBehaviour
{
    [SerializeField]
    bool invert = false;
    [Range(0f, 90f)]
    [SerializeField]
    float upperLimit = 60f;
    [Range(0f, 90f)]
    [SerializeField]
    float lowerLimit = 60f;
    [SerializeField]
    float yawSpeed = 10f;
    [SerializeField]
    float pitchSpeed = 2f;

    private Transform thisTransform;
    private Transform camTransform;
    private float xAngle;
    private float yAngle;
    private bool canLook = true;
    private PhotonView view;

    private void Awake()
    {
        thisTransform = transform;
        camTransform = GetComponentInChildren<Camera>().transform;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(InputBridgeBase.instance == null)
        {
            Debug.LogError("Mouse Cam Controller cannot find input bridge!", this);
        }
        xAngle = camTransform.localRotation.eulerAngles.x;
        yAngle = thisTransform.localRotation.eulerAngles.y;
    }

    private void Update()
    {
        if (!canLook) return;
        Pitch();
        Yaw();
    }

    // move cam up/down
    private void Pitch()
    {
        float input = invert ? InputBridgeBase.instance.PitchAxis : -InputBridgeBase.instance.PitchAxis;
        xAngle += input * pitchSpeed;
        xAngle = Mathf.Clamp(xAngle, -upperLimit, lowerLimit);
        camTransform.localRotation = Quaternion.Euler(new Vector3(xAngle, 0, 0));
    }

    // rotate gameobject
    private void Yaw()
    {
        float input = InputBridgeBase.instance.YawAxis;
        yAngle += input * yawSpeed;
        thisTransform.localRotation = Quaternion.Euler(new Vector3(0,yAngle,0));
    }

    public void ResetView()
    {
        if (!view.IsMine) return;
        canLook = false;
        thisTransform.localRotation = Quaternion.Euler(Vector3.zero);
        camTransform.localRotation = Quaternion.Euler(Vector3.zero);
        xAngle = 0;
        yAngle = 0;
        canLook = true;
    }
}
