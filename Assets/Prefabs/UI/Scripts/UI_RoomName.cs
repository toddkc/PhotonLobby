using UnityEngine;
using UnityEngine.UI;

public class UI_RoomName : MonoBehaviour
{
    [SerializeField] InputField roomNameInput = default;
    private const string playerPrefsKey = "RoomName";

    private void Start()
    {
        string roomname;
        if (PlayerPrefs.HasKey(playerPrefsKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(playerPrefsKey)))
        {
            roomname = PlayerPrefs.GetString(playerPrefsKey);
        }
        else
        {
            roomname = "Room" + Random.Range(11111111, 99999999);
        }
        roomNameInput.text = roomname;
    }

    private void OnEnable()
    {
        roomNameInput.onValueChanged.AddListener(UpdateRoomName);
    }

    private void OnDisable()
    {
        roomNameInput.onValueChanged.RemoveListener(UpdateRoomName);
    }

    // update photon and playerprefs when name changed
    private void UpdateRoomName(string name)
    {
        PlayerPrefs.SetString(playerPrefsKey, name);
        Debug.Log("room name set as: " + name);
    }
}