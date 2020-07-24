using Photon.Realtime;

public static class CustomRoomProperties
{
    public const string teams = "teams";
    public const string scores = "scores";
    public const string game = "game";

    public static int[] GetTeams(this Room room)
    {
        return (int[])room.CustomProperties[teams];
    }

    public static int[] GetScores(this Room room)
    {
        return (int[])room.CustomProperties[scores];
    }

    public static int GetGameScene(this Room room)
    {
        return (int)room.CustomProperties[game];
    }

    public static int[] AddToTeam(this Room room, int teamIndex, int value)
    {
        int[] _teams = room.GetTeams();
        _teams[teamIndex] += value;

        room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { teams, _teams } });
        return _teams;
    }

    public static int[] AddScore(this Room room, int teamIndex, int value)
    {
        int[] _scores = room.GetScores();
        _scores[teamIndex] += value;

        room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { scores, _scores } });
        return _scores;
    }

    public static void SetGameScene(this Room room, int value)
    {
        room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { game, value } });
    }
}
