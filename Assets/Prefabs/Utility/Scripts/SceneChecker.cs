using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// utility so I don't have to keep switching to the menu scene when testing games
/// </summary>

public class SceneChecker : MonoBehaviour
{
    private void Awake()
    {
        if(!PhotonNetwork.InRoom)
        {
            SceneManager.LoadScene(0);
        }
    }
}