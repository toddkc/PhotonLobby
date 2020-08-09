using System.Collections.Generic;
using UnityEngine;

public class CameraRigItemSetup : MonoBehaviour
{
    [SerializeField]
    private List<Transform> left = new List<Transform>();
    [SerializeField]
    private List<Transform> right = new List<Transform>();

    private void Start()
    {
        var _switcher = GetComponentInParent<ItemSwitch>();
        foreach (Transform trans in left)
        {
            _switcher.leftHandPlayerItems.Add(trans.GetComponent<IGrabbable>());
        }
        foreach (Transform trans in right)
        {
            _switcher.rightHandPlayerItems.Add(trans.GetComponent<IGrabbable>());
        }
    }
}