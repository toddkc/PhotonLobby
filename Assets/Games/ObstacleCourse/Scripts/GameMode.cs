using Photon.Pun;
using Photon.Realtime;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    [SerializeField] protected float lobbyDelay = default;
    [SerializeField] protected float startDelay = default;
    [SerializeField] protected GameEvent stopGameEvent = default;
    [SerializeField] protected GameEvent startGameEvent = default;
    [SerializeField] protected StringReference uiMessage = default;
    [SerializeField] protected GameObject avatar = default;

    public static GameMode instance;
    public List<Team> teams;

    public abstract bool IsGameActive { get; set; }

    public abstract void PlayerScored(Player player, int value);

    public abstract void SetupAndSpawn();

    public abstract int CheckScores();

    public virtual IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(lobbyDelay);
        stopGameEvent.Raise();
    }

    public IEnumerator StartGameCountdown()
    {
        yield return new WaitForSeconds(startDelay);
        CustomRoomProperties.SetGameState(PhotonNetwork.CurrentRoom, 2);
    }

    public abstract void SpawnAvatar(int teamIndex);
}