using UnityEngine;

public class GameEventTester : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (GameManager.instance == null) return;
            GameManager.instance.PlayerScored();
        }
    }
}