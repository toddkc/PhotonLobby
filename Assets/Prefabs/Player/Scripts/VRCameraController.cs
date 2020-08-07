using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VRCameraController : MonoBehaviour
{
    [SerializeField] private Transform transformToRotate = default;
    [SerializeField] private float step = 45;
    private PhotonView view;
    public bool canLook = true;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneChange;
    }

    private void Update()
    {
        if (!canLook) return;
        Rotate();
    }

    private void Rotate()
    {
        float _curr = transformToRotate.localEulerAngles.y;
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight))
        {
            _curr += step;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft))
        {
            _curr -= step;
        }

        transformToRotate.localRotation = Quaternion.Euler(new Vector3(0, _curr, 0));
    }

    private void SceneChange(Scene oldscene, Scene newscene)
    {
        HardResetView();
    }

    public void HardResetView()
    {
        if (!view.IsMine) return;
        canLook = false;
        transformToRotate.localRotation = Quaternion.Euler(Vector3.zero);
        OVRManager.display.RecenterPose();
        canLook = true;
    }

    public void SoftResetView()
    {
        if (!view.IsMine) return;
        canLook = false;
        OVRManager.display.RecenterPose();
        canLook = true;
    }
}