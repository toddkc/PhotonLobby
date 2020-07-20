using Photon.Realtime;

public static class CustomPlayerProperties
{
    public static void UpdateProps<T>(Player player, string key, T value)
    {
        ExitGames.Client.Photon.Hashtable props = player.CustomProperties;
        if (props.ContainsKey(key)) props.Remove(key);
        props.Add(key, value);
        player.SetCustomProperties(props);
    }

    public static void ResetProps(Player player)
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        player.SetCustomProperties(props);
    }
}
