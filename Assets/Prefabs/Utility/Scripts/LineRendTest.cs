using UnityEngine;

public class LineRendTest : MonoBehaviour
{
    [SerializeField] Transform shoulder = default;
    [SerializeField] Transform controller = default;
    private LineRenderer rend;

    private void Awake()
    {
        rend = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        var _heading = controller.position - shoulder.position;
        var _line = shoulder.position + _heading.normalized * 10;
        rend.SetPosition(0, shoulder.position);
        rend.SetPosition(1, _line);
    }

    private void PrintTransforms()
    {
        Debug.LogError("pos" + controller.position);
        Debug.LogError("ctpos" + controller.TransformPoint(Vector3.zero));
        Debug.LogError("thistpos" + transform.TransformPoint(controller.position));
    }
}