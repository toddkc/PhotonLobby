using UnityEngine;

/// <summary>
/// Menus open/close on a keypress, this listens for event.
/// </summary>

public class ToggleActiveOnEvent : MonoBehaviour
{
    [SerializeField] private GameObject toggleObject = default;

    public void Toggle()
    {
        toggleObject.SetActive(!toggleObject.activeSelf);
    }
}