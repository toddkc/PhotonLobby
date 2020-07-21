using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// custom world-space inputfield
/// </summary>

public class CustomUIInputField : MonoBehaviour, ICustomUIElement
{
    private InputField inputField;
    private bool isActive = false;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    private void Update()
    {
        if (isActive)
        {
            if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Escape) || InputBridgeBase.instance.Interact)
            {
                inputField.DeactivateInputField();
                OnExit();
            }
        }
    }

    public void OnExit()
    {
        inputField.image.color = inputField.colors.normalColor;
    }

    public void OnHover()
    {
        inputField.image.color = inputField.colors.highlightedColor;
    }

    public void OnClick()
    {
        inputField.image.color = inputField.colors.selectedColor;
        inputField.ActivateInputField();
        isActive = true;
    }
}
