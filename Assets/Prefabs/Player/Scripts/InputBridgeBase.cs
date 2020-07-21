using UnityEngine;

/// <summary>
/// base class for the input bridge, inherit to create specific input for kbm or oculus
/// </summary>
public class InputBridgeBase : MonoBehaviour
{
    public static InputBridgeBase instance;

    public float MoveAxis { get; protected set; }
    public float StrafeAxis { get; protected set; }
    public float PitchAxis { get; protected set; }
    public float YawAxis { get; protected set; }
    public bool Interact { get; protected set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MoveAxis = 0;
        StrafeAxis = 0;
        PitchAxis = 0;
        YawAxis = 0;
        Interact = false;
    }
}
