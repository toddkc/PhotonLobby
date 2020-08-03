using Photon.Pun;
using ScriptableObjectArchitecture;
using UnityEngine;

/// <summary>
/// component used to spawn in required lobby scene items
/// this makes it easier to copy a lobby scene template
/// </summary>

public class LobbySetup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private BoolReference isVR = default;

    [Header("Shared Prefabs")]
    [SerializeField] private GameObject uiPlayerDisplay;
    [SerializeField] private Vector3 playerDisplayLocation;
    [SerializeField] private float playerDisplayRotation;
    [SerializeField] private GameObject uiHostPanel;
    [SerializeField] private Vector3 hostPanelLocation;
    [SerializeField] private float hostPanelRotation;
    [SerializeField] private GameObject uiGuestPanel;
    [SerializeField] private Vector3 guestPanelLocation;
    [SerializeField] private float guestPanelRotation;

    [Header("PC Prefabs")]
    [SerializeField] private GameObject uiMessagesPC;
    [SerializeField] private GameObject avatarPC;

    [Header("VR Prefabs")]
    [SerializeField] private GameObject avatarVR;

    private void Start()
    {
        var pd = Instantiate(uiPlayerDisplay);
        pd.transform.position = playerDisplayLocation;
        pd.transform.rotation = Quaternion.Euler(0, playerDisplayRotation, 0);

        var hp = Instantiate(uiHostPanel);
        hp.transform.position = hostPanelLocation;
        hp.transform.rotation = Quaternion.Euler(0, hostPanelRotation, 0);

        var gp = Instantiate(uiGuestPanel);
        gp.transform.position = guestPanelLocation;
        gp.transform.rotation = Quaternion.Euler(0, guestPanelRotation, 0);

        SetupVR();
        SetupPC();
    }

    private void SetupVR()
    {
        if (!isVR.Value) return;
        PhotonNetwork.Instantiate(avatarVR.name,Vector3.zero,Quaternion.identity);
    }

    private void SetupPC()
    {
        if (isVR.Value) return;
        Instantiate(uiMessagesPC);
        PhotonNetwork.Instantiate(avatarPC.name, Vector3.zero, Quaternion.identity);
    }
}