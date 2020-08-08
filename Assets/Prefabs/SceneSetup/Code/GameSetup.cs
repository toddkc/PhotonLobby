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
    [SerializeField] private GameObject uiPlayerDisplay;
    [SerializeField] private Vector3 playerDisplayLocation;
    [SerializeField] private float playerDisplayRotation;
    [SerializeField] private GameObject uiGamePanel;

    [Header("PC Prefabs")]
    [SerializeField] private GameObject uiMessagesPC;

    private void Start()
    {
        Instantiate(uiGamePanel);
        SetupPC();
    }

    private void SetupPC()
    {
        if (isVR.Value) return;
        Instantiate(uiMessagesPC);

        var pd = Instantiate(uiPlayerDisplay);
        pd.transform.position = playerDisplayLocation;
        pd.transform.rotation = Quaternion.Euler(0, playerDisplayRotation, 0);
    }
}