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
        if (playerCamera == null) return;
        var _worldpos = playerCamera.position;
        thisTransform.position = _worldpos;
        var _direction = Vector3.ProjectOnPlane(playerCamera.forward, thisTransform.up).normalized * offset;
        thisTransform.position += _direction;
        thisTransform.LookAt(playerCamera);
        toggleObject.SetActive(true);
    }

    private void Awake()
    {
        thisTransform = transform;
        delay = new WaitForSeconds(0.2f);
        StartCoroutine(FindCamera());
    }

    private void Start()
    {
        toggleObject.SetActive(false);
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