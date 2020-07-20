using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerNickname : MonoBehaviourPun
{
    [SerializeField] InputField nicknameInput = default;
    private const string playerPrefsKey = "nickname";

    private void Start()
    {
        string nickname;
        if (PlayerPrefs.HasKey(playerPrefsKey) && !string.IsNullOrEmpty(PlayerPrefs.GetString(playerPrefsKey)))
        {
            nickname = PlayerPrefs.GetString(playerPrefsKey);
        }
        else
        {
            nickname = "Player" + Random.Range(11111111, 99999999);
        }
        nicknameInput.text = nickname;
        PhotonNetwork.NickName = nickname;
    }

    //private void OnEnable()
    //{
    //    nicknameInput.onValueChanged.AddListener(UpdatePlayerNickname);
    //}

    //private void UpdatePlayerNickname(string nickname)
    //{
    //    PlayerPrefs.SetString(playerPrefsKey, nickname);
    //    PhotonNetwork.NickName = nickname;
    //    Debug.Log("player nickname set as: " + nickname);
    //}
}
