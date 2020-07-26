using UnityEngine;

/// <summary>
/// experimenting with different menus so I'd like to be able to spawn everything via code
/// </summary>

public class MainMenuSetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isVR = false;

    [Header("Shared Prefabs")]
    public GameObject networkManager;
    public GameObject voiceManager;
    public GameObject eventTester;
    public GameObject sceneLoader;

    [Header("PC Prefabs")]
    public GameObject inputBridgePC;
    public GameObject uiMenuPC;
    public GameObject uiMessagesPC;
    public GameObject avatarPC;

    [Header("VR Prefabs")]
    public GameObject inputBridgeVR;
    public GameObject uiMenuVR;
    public GameObject uiMessagesVR;
    public GameObject avatarVR;

    private void Start()
    {
        Instantiate(networkManager);
        Instantiate(voiceManager);
        Instantiate(eventTester);
        Instantiate(sceneLoader);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR) return;
        Instantiate(inputBridgeVR);
        Instantiate(uiMenuVR);
        Instantiate(uiMessagesVR);
        Instantiate(avatarVR);
    }

    private void SetupPC()
    {
        if (isVR) return;
        Instantiate(inputBridgePC);
        Instantiate(uiMenuPC);
        Instantiate(uiMessagesPC);
        Instantiate(avatarPC);
    }
}