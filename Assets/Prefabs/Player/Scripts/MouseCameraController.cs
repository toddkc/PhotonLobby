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
    public Transform camTransform;
    private float xAngle;
    private float yAngle;
    public bool canLook = true;
    private PhotonView view;

    private void Awake()
    {
        thisTransform = transform;
        view = GetComponentInParent<PhotonView>();
        //camTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        if (InputBridgeBase.instance == null)
        {
            Debug.LogError("Mouse Cam Controller cannot find input bridge!", this);
        }
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        yield return null;
        while (camTransform == null)
        {
            yield return null;
            camTransform = GetComponentInChildren<Camera>().transform;
            xAngle = camTransform.localRotation.eulerAngles.x;
            yAngle = thisTransform.localRotation.eulerAngles.y;
        }
        yield return null;
    }

    private void Update()
    {
        if (!canLook) return;
        float _inputHorizontal = InputBridgeBase.instance.YawAxis;
        float _inputVertical = invert ? InputBridgeBase.instance.PitchAxis : -InputBridgeBase.instance.PitchAxis;
        RotateCamera(_inputHorizontal, _inputVertical);
    }

    private void RotateCamera(float _newHorizontalInput, float _newVerticalInput)
    {
        yAngle += _newHorizontalInput;
        xAngle += _newVerticalInput;
        xAngle = Mathf.Clamp(xAngle, -upperLimit, lowerLimit);
        thisTransform.localRotation = Quaternion.Euler(new Vector3(0, yAngle, 0));
        camTransform.localRotation = Quaternion.Euler(new Vector3(xAngle, 0, 0));
    }

    public void ResetView()
    {
        if (!view.IsMine) return;
        canLook = false;
        xAngle = 0;
        yAngle = 0;
        thisTransform.localRotation = Quaternion.Euler(Vector3.zero);
        canLook = true;
    }
}
