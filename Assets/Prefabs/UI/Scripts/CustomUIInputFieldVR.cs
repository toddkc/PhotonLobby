using UnityEngine;
using UnityEngine.UI;
using VRKeys;

public class CustomUIInputFieldVR : MonoBehaviour, ICustomUIElement
{
    [SerializeField] private Keyboard keyboard = default;
    [SerializeField] private Text placeholderText = default;
    private InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<InputField>();
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
        Debug.LogError("input clicked");
        inputField.image.color = inputField.colors.selectedColor;
        keyboard.Enable();
        keyboard.SetText("");
        keyboard.SetPlaceholderMessage(placeholderText.text);
        keyboard.OnUpdate.AddListener(HandleUpdate);
        keyboard.OnSubmit.AddListener(HandleSubmit);
        keyboard.OnCancel.AddListener(HandleCancel);
    }

    private void HandleUpdate(string text)
    {
        inputField.text = text;
    }
    private void HandleSubmit(string text)
    {
        keyboard.DisableInput();
        keyboard.OnUpdate.RemoveListener(HandleUpdate);
        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);
        keyboard.Disable();
        OnExit();
    }
    private void HandleCancel()
    {
        keyboard.DisableInput();
        keyboard.OnUpdate.RemoveListener(HandleUpdate);
        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);
        keyboard.Disable();
        OnExit();
    }
}