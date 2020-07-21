using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this is used to show various messages in-game
/// </summary>

public class UIMessageDisplay : MonoBehaviour
{
    //[SerializeField] GameObject panel = default;
    [SerializeField] Text messageText = default;
    [SerializeField] float messageDelay = default;
    private WaitForSeconds delay;

    private void Start()
    {
        PlayerPrefs.SetString("message", "test");
        delay = new WaitForSeconds(messageDelay);
    }

    public void DisplayMessage()
    {
        StopAllCoroutines();
        string message = PlayerPrefs.GetString("message");
        if (!string.IsNullOrEmpty(message))
        {
            messageText.text = message;
            //panel.SetActive(true);
            StartCoroutine(DelayHideMessage());
        }
    }

    private IEnumerator DelayHideMessage()
    {
        yield return delay;
        messageText.text = "";
        //panel.SetActive(false);
    }
}
