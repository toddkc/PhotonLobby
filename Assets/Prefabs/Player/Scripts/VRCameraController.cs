using Photon.Pun;
using UnityEngine;

public class VRCameraController : MonoBehaviour
{
    [SerializeField] private Transform transformToRotate = default;
    [SerializeField] private float step = 45;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        //var _rotation = transformToRotate.eulerAngles;
        float _curr = transformToRotate.eulerAngles.y;

        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
        {
            //_rotation.y += step;
            _curr += step;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
        {
            //_rotation.y -= step;
            _curr -= step;
        }

        transformToRotate.localRotation = Quaternion.Euler(new Vector3(0, _curr, 0));
        //transformToRotate.eulerAngles = _rotation;
    }

    public void ResetView()
    {
        if (!view.IsMine) return;
        transformToRotate.localRotation = Quaternion.Euler(Vector3.zero);
        OVRManager.display.RecenterPose();
    }
}