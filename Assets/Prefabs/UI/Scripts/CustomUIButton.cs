using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// custom world-space button
/// </summary>

public class CustomUIButton : MonoBehaviour, ICustomUIElement
{
    [SerializeField] private AudioClip clickAudio = default;
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
        if (clickAudio) AudioManager.instance.PlayClip(clickAudio);
        button.image.color = button.colors.normalColor;
        button.onClick.Invoke();
    }
}
