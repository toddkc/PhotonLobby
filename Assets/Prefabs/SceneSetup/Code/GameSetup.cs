using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isVR = false;

    [Header("Shared Prefabs")]
    public GameObject uiPlayerDisplay;
    public GameObject uiGamePanel;

    [Header("PC Prefabs")]
    public GameObject uiMessagesPC;

    [Header("VR Prefabs")]
    public GameObject uiMessagesVR;

    private void Start()
    {
        Instantiate(uiPlayerDisplay);
        Instantiate(uiGamePanel);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR) return;
        Instantiate(uiMessagesVR);
    }

    private void SetupPC()
    {
        if (isVR) return;
        Instantiate(uiMessagesPC);
    }
}