using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// static helper class that is used to update and reset attributes stored on the Photon Player level
/// </summary>

public static class CustomPlayerProperties
{
    public const string score = "score";
    public const string team = "team";

    // player score
    public static int GetCurrentScore(this PhotonView player)
    {
        return player.Owner.GetCurrentScore();
    }
    public static int GetCurrentScore(this Player player)
    {
        return (int)player.CustomProperties[score];
    }
    public static void AddScore(this PhotonView player, int value)
    {
        player.Owner.AddScore(value);
    }
    public static void AddScore(this Player player, int value)
    {
        var _score = GetCurrentScore(player);
        _score += value;
        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { score, _score } });
    }

    // player team
    public static int GetTeam(this PhotonView player)
    {
        return player.Owner.GetTeam();
    }
    public static int GetTeam(this Player player)
    {
        return (int)player.CustomProperties[team];
    }
    public static void SetTeam(this PhotonView player, int value)
    {
        player.Owner.SetTeam(value);
    }
    public static void SetTeam(this Player player, int value)
    {
        var _team = new ExitGames.Client.Photon.Hashtable() { { team, value } };
        player.SetCustomProperties(_team);
    }

    // reset
    public static void ResetProps(this PhotonView player)
    {
        player.Owner.ResetProps();
    }
    public static void ResetProps(this Player player)
    {
        var _props = new ExitGames.Client.Photon.Hashtable()
        {
            { score, 0 },
            { team,  0 }
        };
        player.SetCustomProperties(_props);
    }
}
