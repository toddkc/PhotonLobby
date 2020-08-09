using UnityEngine;
using UnityEngine.UI;

public class CustomUICheckBoxVR : MonoBehaviour, ICustomUIElement
{
    [SerializeField] private AudioClip clickAudio = default;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    // simulate mouse exiting hover
    public void OnExit()
    {
        toggle.image.color = toggle.colors.normalColor;
    }

    // simulate mouse entering hover
    public void OnHover()
    {
        toggle.image.color = toggle.colors.highlightedColor;
    }

    // when object is clicked
    public void OnClick()
    {
        if (clickAudio) AudioManager.instance.PlayClip(clickAudio);
        toggle.image.color = toggle.colors.normalColor;
        toggle.isOn = !toggle.isOn;
    }
}