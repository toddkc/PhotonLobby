using UnityEngine;

/// <summary>
/// one place to store all the input values that may be needed elsewhere
/// </summary>

public class InputBridgePC : MonoBehaviour
{
    public static InputBridgePC instance;

    [SerializeField] private string moveAxis = "Vertical";
    [SerializeField] private string strafeAxis = "Horizontal";
    [SerializeField] private string pitchAxis = "Mouse Y";
    [SerializeField] private string yawAxis = "Mouse X";
    [SerializeField] private KeyCode interactKey = default;
    [SerializeField] private int mouseInteract = 0;

    public float MoveAxis { get; private set; }
    public float StrafeAxis { get; private set; }
    public float PitchAxis { get; private set; }
    public float YawAxis { get; private set; }
    public bool Interact { get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        MoveAxis = Input.GetAxis(moveAxis);
        StrafeAxis = Input.GetAxis(strafeAxis);
        PitchAxis = Input.GetAxis(pitchAxis);
        YawAxis = Input.GetAxis(yawAxis);

        Interact = Input.GetKey(interactKey) || Input.GetMouseButton(mouseInteract);
    }
}
