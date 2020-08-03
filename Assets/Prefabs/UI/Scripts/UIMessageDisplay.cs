using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this is used to show various messages in-game
/// </summary>

public class UIMessageDisplay : MonoBehaviour
{
    [SerializeField] private Text messageText = default;
    [SerializeField] private StringReference messageVariable = default;
    [SerializeField] private GameObject messagePanel = default;
    [SerializeField] private float preDelayTime = default;
    [SerializeField] private float postDelayTime = default;
    private WaitForSeconds preDelay;
    private WaitForSeconds postDelay;

    private void Start()
    {
        preDelay = new WaitForSeconds(preDelayTime);
        postDelay = new WaitForSeconds(postDelayTime);
        messagePanel.SetActive(false);
    }

    // listens for an event and reads the new message to display
    public void DisplayMessage()
    {
        StopAllCoroutines();
        messageText.text = messageVariable.Value;
        StartCoroutine(DelayHideMessage());
        //string message = PlayerPrefs.GetString("message");
        //if (!string.IsNullOrEmpty(message))
        //{
        //    messagePanel.SetActive(false);
        //    messageText.text = message;
        //    StartCoroutine(DelayHideMessage());
        //}
    }

    // optionally hide the message panel after a delay
    private IEnumerator DelayHideMessage()
    {
        messagePanel.SetActive(false);
        yield return preDelay;
        messagePanel.SetActive(true);
        yield return postDelay;
        messagePanel.SetActive(false);
        //if (hidePanel)
        //{
        //    messagePanel.SetActive(false);
        //    yield return preDelay;
        //    messagePanel.SetActive(true);
        //}
        //yield return postDelay;
        //messageText.text = "";
        //if (hidePanel) messagePanel.SetActive(false);
    }
}
