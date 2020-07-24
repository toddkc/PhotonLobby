using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// static helper class that is used to update and reset attributes stored on the Photon Player level
/// </summary>

public static class CustomPlayerProperties
{
    public const string score = "score";
    public const string game = "game";
    public const string team = "team";

    public static int GetCurrentScore(this PhotonView player)
    {
        return player.Owner.GetCurrentScore();
    }
    public static int GetCurrentScore(this Player player)
    {
        return (int)player.CustomProperties[score];
    }

    public static int GetGameScene(this PhotonView player)
    {
        return player.Owner.GetGameScene();
    }
    public static int GetGameScene(this Player player)
    {
        return (int)player.CustomProperties[game];
    }

    public static int GetTeam(this PhotonView player)
    {
        return player.Owner.GetTeam();
    }
    public static int GetTeam(this Player player)
    {
        return (int)player.CustomProperties[team];
    }

    public static void SetScore(this PhotonView player, int value)
    {
        player.Owner.SetScore(value);
    }
    public static void SetScore(this Player player, int value)
    {
        var _score = new ExitGames.Client.Photon.Hashtable() { { score, (byte)value } };
        player.SetCustomProperties(_score);
    }

    public static void SetGameScene(this PhotonView player, int value)
    {
        player.Owner.SetGameScene(value);
    }
    public static void SetGameScene(this Player player, int value)
    {
        var _game = new ExitGames.Client.Photon.Hashtable() { { game, (byte)value } };
        player.SetCustomProperties(_game);
    }

    public static void SetTeam(this PhotonView player, int value)
    {
        player.Owner.SetTeam(value);
    }
    public static void SetTeam(this Player player, int value)
    {
        var _team = new ExitGames.Client.Photon.Hashtable() { { team, (byte)value } };
        player.SetCustomProperties(_team);
    }

    public static void ResetProps(this PhotonView player)
    {
        player.Owner.ResetProps();
    }

    public static void ResetProps(this Player player)
    {
        var _props = new ExitGames.Client.Photon.Hashtable()
        {
            { score, (byte)0 },
            { team,  (byte)0 },
            { game,  (byte)0 }
        };
        player.SetCustomProperties(_props);
    }
}
