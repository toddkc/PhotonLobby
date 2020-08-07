using System.Collections;
using UnityEngine;

public class ControllerHaptic : MonoBehaviour
{
    [SerializeField] private float pulseTime = 0.2f;
    private WaitForSeconds pulseDelay;

    private void Awake()
    {
        pulseDelay = new WaitForSeconds(pulseTime);
    }

    public void Pulse(OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(0.2f, 0.2f, controller);
        StartCoroutine(ResetHaptic(controller));
    }

    private IEnumerator ResetHaptic(OVRInput.Controller controller)
    {
        yield return pulseDelay;
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}