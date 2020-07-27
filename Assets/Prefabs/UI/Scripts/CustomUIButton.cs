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

    // simulate mouse exiting hover
    public void OnExit()
    {
        button.image.color = button.colors.normalColor;
    }

    // simulate mouse entering hover
    public void OnHover()
    {
        button.image.color = button.colors.highlightedColor;
    }

    // when object is clicked
    public void OnClick()
    {
        button.image.color = button.colors.normalColor;
        button.onClick.Invoke();
    }
}
