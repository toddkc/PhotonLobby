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
    [SerializeField] float preDelayTime = default;
    [SerializeField] float postDelayTime = default;
    private WaitForSeconds preDelay;
    private WaitForSeconds postDelay;

    private void Start()
    {
        PlayerPrefs.SetString("message", "test");
        preDelay = new WaitForSeconds(preDelayTime);
        postDelay = new WaitForSeconds(postDelayTime);
        if (hidePanel) messagePanel.SetActive(false);
    }

    // listens for an event and reads the new message to display
    public void DisplayMessage()
    {
        StopAllCoroutines();
        string message = PlayerPrefs.GetString("message");
        if (!string.IsNullOrEmpty(message))
        {
            messagePanel.SetActive(false);
            messageText.text = message;
            StartCoroutine(DelayHideMessage());
        }
    }

    // optionally hide the message panel after a delay
    private IEnumerator DelayHideMessage()
    {
        if (hidePanel)
        {
            messagePanel.SetActive(false);
            yield return preDelay;
            messagePanel.SetActive(true);
        }
        yield return postDelay;
        messageText.text = "";
        if (hidePanel) messagePanel.SetActive(false);
    }
}
