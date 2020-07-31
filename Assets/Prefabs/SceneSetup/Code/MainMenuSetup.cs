using UnityEngine;
using ScriptableObjectArchitecture;

/// <summary>
/// component used to spawn in required menu scene items
/// this makes it easier to copy a menu scene template
/// </summary>

public class MainMenuSetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoolReference isVR = default;

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
        if (!isVR.Value) return;
        Instantiate(inputBridgeVR);
        Instantiate(uiMenuVR);
        Instantiate(uiMessagesVR);
        Instantiate(avatarVR);
    }

    private void SetupPC()
    {
        if (isVR.Value) return;
        Instantiate(inputBridgePC);
        Instantiate(uiMenuPC);
        Instantiate(uiMessagesPC);
        Instantiate(avatarPC);
    }
}