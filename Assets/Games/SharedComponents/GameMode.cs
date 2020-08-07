using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    public BoolReference isVrBuild = default;
    public static GameMode instance;
    public abstract bool IsGameActive { get; }
    public List<Team> Teams;

    public abstract void StartGame();
    public abstract void CheckGame();
    public abstract void EndGame();
    protected abstract void ResetGame();
    public abstract void SetupAvatars();
}