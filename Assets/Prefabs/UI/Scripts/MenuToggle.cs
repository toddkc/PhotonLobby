using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject toggleObject = default;

    public void Toggle()
    {
        toggleObject.SetActive(!toggleObject.activeSelf);
    }
}