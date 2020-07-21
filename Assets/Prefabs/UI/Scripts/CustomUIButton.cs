using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// custom world-space button
/// </summary>

public class CustomUIButton : MonoBehaviour, ICustomUIElement
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnExit()
    {
        button.image.color = button.colors.normalColor;
    }

    public void OnHover()
    {
        button.image.color = button.colors.highlightedColor;
    }

    public void OnClick()
    {
        button.image.color = button.colors.pressedColor;
        button.onClick.Invoke();
    }
}
