namespace NetworkTutorial
{
    using Photon.Pun;
    using NetworkTutorial.GameEvents;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoadedCheck : MonoBehaviour
    {
        [SerializeField] GameEvent allPlayersInSceneEvent = default;
        private bool allPlayersLoaded = false;

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var playersInRoom = PhotonNetwork.CurrentRoom.Players;
            var scene = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(VerifySceneLoad(scene));
        }

        private IEnumerator VerifySceneLoad(int index)
        {
            while (!allPlayersLoaded)
            {
                var playersInRoom = PhotonNetwork.CurrentRoom.Players;
                bool loaded = true;
                foreach (var player in playersInRoom)
                {
                    Debug.Log(player.Value.NickName + " has loaded scene: " + player.Value.CustomProperties.ContainsKey("loaded_scene"));
                    if (!player.Value.CustomProperties.ContainsKey("loaded_scene"))
                    {
                        loaded = false;
                    }
                }
                if (loaded)
                {
                    allPlayersLoaded = true;
                }
                yield return null;
            }
            Debug.Log("all players loaded scene");
            allPlayersInSceneEvent.Raise();
        }
    }
}