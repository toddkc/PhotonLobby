using Photon.Realtime;
using UnityEngine;

public class ObstacleCourseGameMode : GameMode
{
    public override bool IsGameActive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }
    public override void PlayerScored(Player player, int value)
    {
        Debug.LogError("scored!");
    }
}