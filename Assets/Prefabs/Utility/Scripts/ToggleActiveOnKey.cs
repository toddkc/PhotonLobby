using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;

/// <summary>
/// Menus open/close on a keypress, this fires the event.
/// </summary>

public class ToggleActiveOnKey : MonoBehaviour
{
    [SerializeField] private float delayTime = 0.2f;
    [SerializeField] private GameEvent toggleMenu = default;
    [SerializeField] private GameEvent toggleScore = default;

    private WaitForSeconds delay;
    private bool canClick = true;

    private void Awake()
    {
        delay = new WaitForSeconds(delayTime);
    }

    private void Update()
    {
        if (InputBridgeBase.instance.Menu && canClick)
        {
            StartCoroutine(ResetClick());
            toggleMenu.Raise();
        }

        if (InputBridgeBase.instance.Score && canClick)
        {
            StartCoroutine(ResetClick());
            toggleScore.Raise();
        }
    }

    private IEnumerator ResetClick()
    {
        Debug.LogError("test first");
        canClick = false;
        yield return delay;
        canClick = true;
    }
}