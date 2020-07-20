using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public static class PUN_Events
{
    public const byte LoadLevelEventCode = 1;
    public const byte UnloadLevelEventCode = 2;
    public const byte StartGameEventCode = 3;
    public const byte ResetGameEventCode = 4;
    public const byte StopGameEventCode = 5;

    public static void LoadLevelEvent(int buildIndex)
    {
        object[] content = new object[] { buildIndex };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(LoadLevelEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void UnloadLevelEvent()
    {
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(UnloadLevelEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void StartGameEvent()
    {
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(StartGameEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void ResetGameEvent()
    {
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(ResetGameEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void StopGameEvent()
    {
        object[] content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(StopGameEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
