using UnityEngine;

[CreateAssetMenu(fileName ="Scene Details")]
public class SceneDetails : ScriptableObject
{
    public string scene;
    public int buildIndex = 0;
    public int maxPlayers = 4;
    public int minPlayers = 1;
}