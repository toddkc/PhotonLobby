using System.Collections;
using UnityEngine;

public class MoveToPlayerOnEnable : MonoBehaviour
{
    [SerializeField] private float offset = 5;
    [SerializeField] private GameObject toggleObject = default;
    private Transform playerCamera;
    private Transform thisTransform;
    private WaitForSeconds delay;

    public void Toggle()
    {
        if (toggleObject.activeSelf)
        {
            toggleObject.SetActive(false);
            return;
        }

        Debug.LogError("test middle");
        if (playerCamera == null) return;

        // move menu to camera
        var _worldpos = playerCamera.position;
        thisTransform.position = _worldpos;

        // move forward by offset
        var _direction = Vector3.ProjectOnPlane(playerCamera.forward, thisTransform.up).normalized * offset;
        thisTransform.position += _direction;

        // look at player
        thisTransform.LookAt(playerCamera);

        toggleObject.SetActive(true);

        Debug.LogError("test last");
    }

    private void Awake()
    {
        thisTransform = transform;
        delay = new WaitForSeconds(0.2f);
        StartCoroutine(FindCamera());
    }

    private IEnumerator FindCamera()
    {
        int counter = 0;
        while (counter < 10 && playerCamera == null)
        {
            counter++;
            if(Camera.main != null)
            {
                playerCamera = Camera.main.transform;
            }
            yield return delay;
        }
    }
}