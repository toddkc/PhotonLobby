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
    [SerializeField] private GameObject networkManager;
    [SerializeField] private GameObject voiceManager;
    [SerializeField] private GameObject eventTester;
    [SerializeField] private GameObject sceneLoader;

    [Header("PC Prefabs")]
    [SerializeField] private GameObject inputBridgePC;
    [SerializeField] private GameObject uiMessagesPC;
    [SerializeField] private GameObject uiMenuPC;
    [SerializeField] private GameObject avatarPC;

    [Header("VR Prefabs")]
    [SerializeField] private GameObject inputBridgeVR;
    [SerializeField] private GameObject uiMenuVR;
    [SerializeField] private GameObject avatarVR;

    private void Start()
    {
        Instantiate(networkManager);
        Instantiate(voiceManager);
        if (eventTester != null) Instantiate(eventTester);
        Instantiate(sceneLoader);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR.Value) return;
        Instantiate(inputBridgeVR);
        Instantiate(uiMenuVR);
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