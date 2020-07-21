using UnityEngine;

/// <summary>
/// lock and hide the mouse while playing
/// </summary>

public class MouseLock : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
