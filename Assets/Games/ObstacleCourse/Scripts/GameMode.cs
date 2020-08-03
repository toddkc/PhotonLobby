using Boo.Lang;
using Photon.Realtime;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    public static GameMode instance;
    public abstract bool IsGameActive { get; set; }
    public List<Team> Teams;

    public abstract void PlayerScored(Player player, int value);
}