using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderUI : MonoBehaviour
{
    [SerializeField] private GameObject loadScenePanel = default;
    [SerializeField] private GameObject changeGamePanel = default;

    private void Start()
    {
        loadScenePanel.SetActive(false);
        changeGamePanel.SetActive(false);
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("join room");
            loadScenePanel.SetActive(true);
            changeGamePanel.SetActive(false);
        }
    }
    public void LeaveRoom()
    {
        Debug.Log("leave room");
        loadScenePanel.SetActive(false);
        changeGamePanel.SetActive(false);
    }
    public void JoinGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("join game");
            loadScenePanel.SetActive(false);
            changeGamePanel.SetActive(false);
        }
    }
    public void LeaveGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
        {
            Debug.Log("leave game");
            loadScenePanel.SetActive(true);
            changeGamePanel.SetActive(false);
        }
    }
}
