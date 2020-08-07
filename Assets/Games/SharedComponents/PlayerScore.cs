using Photon.Pun;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private GameEvent playerScoredEvent = default;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void TriggerScore()
    {
        //Debug.LogError("player triggered score");
        if (!view.IsMine || !GameMode.instance.IsGameActive) return;
        playerScoredEvent.Raise();
    }
}