using UnityEngine;
using UnityEngine.UI;

public class CustomUISlider : MonoBehaviour, ICustomUIElement
{
    [SerializeField] private float interval = default;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // simulate mouse exiting hover
    public void OnExit()
    {
        slider.image.color = slider.colors.normalColor;
    }

    // simulate mouse entering hover
    public void OnHover()
    {
        slider.image.color = slider.colors.highlightedColor;
    }

    // when object is clicked
    public void OnClick()
    {
        // get value
        var _val = slider.value;
        if (slider.value == slider.maxValue)
        {
            slider.value = slider.minValue;
        }
        else if (slider.value + interval <= slider.maxValue)
        {
            slider.value = slider.value + interval;
        }
        else
        {
            slider.value = slider.maxValue;
        }
    }
}