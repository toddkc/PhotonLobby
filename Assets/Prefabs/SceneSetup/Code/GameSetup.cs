using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// component used to spawn in required game scene items
/// this makes it easier to copy a game scene template
/// </summary>

public class GameSetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoolReference isVR = default;

    [Header("Shared Prefabs")]
    public GameObject uiPlayerDisplay;
    public Vector3 playerDisplayLocation;
    public float playerDisplayRotation;
    public GameObject uiGamePanel;
    public Vector3 gamePanelLocation;
    public float gamePanelRotation;

    [Header("PC Prefabs")]
    public GameObject uiMessagesPC;

    [Header("VR Prefabs")]
    public GameObject uiMessagesVR;
    public Vector3 messagesLocation;
    public float messagesRotation;

    private void Start()
    {
        var pd = Instantiate(uiPlayerDisplay);
        pd.transform.position = playerDisplayLocation;
        pd.transform.rotation = Quaternion.Euler(0, playerDisplayRotation, 0);

        var gp = Instantiate(uiGamePanel);
        gp.transform.position = gamePanelLocation;
        gp.transform.rotation = Quaternion.Euler(0, gamePanelRotation, 0);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR.Value) return;
        var uim = Instantiate(uiMessagesVR);
        uim.transform.position = messagesLocation;
        uim.transform.rotation = Quaternion.Euler(0, messagesRotation, 0);
    }

    private void SetupPC()
    {
        if (isVR.Value) return;
        Instantiate(uiMessagesPC);
    }
}