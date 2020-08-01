using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// this ui panel allows the host to select a game scene to load
/// </summary>

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject panel = default;
    [SerializeField] private ScriptableObjectArchitecture.GameEvent hostSelectGameEvent = default;
    [SerializeField] private Transform buttonPanel = default;
    [SerializeField] private GameObject loadGameButton = default;
    [SerializeField] private List<SceneDetails> gameScenes = new List<SceneDetails>();


    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            panel.SetActive(false);
        }
        else
        {
            UpdateGameButtons();
        }
    }

    private void UpdateGameButtons()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // delete all buttons
        DeleteButtons();
        int players = PhotonNetwork.CurrentRoom.PlayerCount;
        // for each scene detail:
        foreach (var sc in gameScenes)
        {
            // if current players in room are valid for game
            if (players >= sc.minPlayers && players <= sc.maxPlayers)
            {
                // create button to load game
                var button = Instantiate(loadGameButton, buttonPanel);
                button.GetComponentInChildren<Text>().text = sc.scene;
                button.GetComponent<Button>().onClick.AddListener
                    (
                        delegate { HostSelectGame(sc.buildIndex); }
                    );
            }
        }
    }

    private void DeleteButtons()
    {
        var buttons = transform.GetComponentsInChildren<Button>().ToArray();
        Debug.LogError("buttons: " + buttons.Length);
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            Destroy(buttons[i].gameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateGameButtons();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateGameButtons();
    }

    // used for ui buttons to load a game scene
    public void HostSelectGame(int index)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PlayerPrefs.SetInt("game", index);
        hostSelectGameEvent.Raise();
    }
}
