using UnityEngine;

/// <summary>
/// world canvas needs to have an assigned camera, which I always forget to do...
/// </summary>

public class CanvasAssignCamera : MonoBehaviour
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        Setup();
    }

    private void OnEnable()
    {
        Setup();
    }

    // check if canvas is worldspace and needs and event camera assigned
    private void Setup()
    {
        if(canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
