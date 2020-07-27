using Photon.Pun;
using UnityEngine;

/// <summary>
/// utility to simulate various game events
/// </summary>

public class GameEventTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            var player = PhotonNetwork.LocalPlayer;
            GameManager.PlayerScored(player, 1);
        }
    }
}