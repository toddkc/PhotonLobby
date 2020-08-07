using UnityEngine;
using UnityEngine.UI;

public class UI_RoomPassword : MonoBehaviour
{
    [SerializeField] InputField passwordInput = default;
    private const string playerPrefsKey = "RoomPassword";

    private void Start()
    {
        string password = "";
        if (PlayerPrefs.HasKey(playerPrefsKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(playerPrefsKey)))
        {
            password = PlayerPrefs.GetString(playerPrefsKey);
        }
        passwordInput.text = password;
    }

    private void OnEnable()
    {
        passwordInput.onValueChanged.AddListener(UpdateRoomPassword);
    }

    private void OnDisable()
    {
        passwordInput.onValueChanged.RemoveListener(UpdateRoomPassword);
    }

    // update photon and playerprefs when name changed
    private void UpdateRoomPassword(string password)
    {
        PlayerPrefs.SetString(playerPrefsKey, password);
        Debug.Log("room password set as: " + password);
    }
}