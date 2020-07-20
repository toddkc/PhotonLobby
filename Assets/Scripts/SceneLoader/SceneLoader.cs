namespace NetworkTutorial
{
    using ExitGames.Client.Photon;
    using NetworkTutorial.GameEvents;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameEvent displayMessageEvent = default;
        [SerializeField] private GameEvent sceneLoadedEvent = default;
        [SerializeField] private GameEvent sceneUnloadedEvent = default;
        private bool isSceneLoaded = false;
        private int? currentGameScene = null;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoadedCallback;
            SceneManager.sceneUnloaded += SceneUnloadedCallback;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneLoadedCallback;
            SceneManager.sceneUnloaded -= SceneUnloadedCallback;
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        public void LeaveRoom()
        {
            Debug.Log("leave room");
            UnloadGameSceneEventResponse();
        }

        private void SceneLoadedCallback(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == currentGameScene)
            {
                sceneLoadedEvent.Raise();
                CustomPlayerProperties.UpdateProps<int>(PhotonNetwork.LocalPlayer, "loaded_scene", scene.buildIndex);
            }
        }

        private void SceneUnloadedCallback(Scene scene)
        {
            if (scene.buildIndex == currentGameScene)
            {
                sceneUnloadedEvent.Raise();
                isSceneLoaded = false;
                currentGameScene = null;
            }
        }

        public void LoadGameScene(int scene)
        {
            if (!isSceneLoaded && PhotonNetwork.IsMasterClient)
            {
                PUN_Events.LoadLevelEvent(scene);
            }
        }

        public void UnloadGameScene()
        {
            if (isSceneLoaded && PhotonNetwork.IsMasterClient)
            {
                PUN_Events.UnloadLevelEvent();
            }
        }

        private void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == PUN_Events.LoadLevelEventCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                int buildIndex = (int)data[0];
                LoadGameSceneEventResponse(buildIndex);
            }
            else if (eventCode == PUN_Events.UnloadLevelEventCode)
            {
                UnloadGameSceneEventResponse();
            }
        }

        private void LoadGameSceneEventResponse(int index)
        {
            if (!isSceneLoaded)
            {
                isSceneLoaded = true;
                currentGameScene = index;
                SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
                PlayerPrefs.SetString("message", "Loading Game...");
                displayMessageEvent.Raise();
                //UIMessageDisplay.instance.DisplayMessage("Loading Game...");
            }
        }

        private void UnloadGameSceneEventResponse()
        {
            if (isSceneLoaded && currentGameScene != null)
            {
                SceneManager.UnloadSceneAsync((int)currentGameScene);
            }
        }
    }
}