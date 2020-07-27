using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this is used to show various messages in-game
/// </summary>

public class UIMessageDisplay : MonoBehaviour
{
    [SerializeField] private bool hidePanel = false;
    [SerializeField] Text messageText = default;
    [SerializeField] GameObject messagePanel = default;
    [SerializeField] float messageDelay = default;
    private WaitForSeconds delay;

    private void Start()
    {
        PlayerPrefs.SetString("message", "test");
        delay = new WaitForSeconds(messageDelay);
        if (hidePanel) messagePanel.SetActive(false);
    }

    // listens for an event and reads the new message to display
    public void DisplayMessage()
    {
        StopAllCoroutines();
        string message = PlayerPrefs.GetString("message");
        if (!string.IsNullOrEmpty(message))
        {
            messageText.text = message;
            if (hidePanel) messagePanel.SetActive(true);
            StartCoroutine(DelayHideMessage());
        }
    }

    // optionally hide the message panel after a delay
    private IEnumerator DelayHideMessage()
    {
        yield return delay;
        messageText.text = "";
        if (hidePanel) messagePanel.SetActive(false);
    }
}
