using System.Collections.Generic;
using UnityEngine;

public class CameraRigItemSetup : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> left = new List<GameObject>();
    [SerializeField]
    private List<GameObject> right = new List<GameObject>();

    private void Start()
    {
        var _switcher = GetComponentInParent<ItemSwitch>();
        foreach (GameObject obj in left)
        {
            _switcher.leftHandPlayerItems.Add(obj);
        }
        foreach (GameObject obj in right)
        {
            _switcher.rightHandPlayerItems.Add(obj);
        }
    }
}