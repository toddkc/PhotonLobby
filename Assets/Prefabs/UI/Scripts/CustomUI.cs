/// <summary>
/// interface to implement a custom world-space UI element
/// </summary>

public interface ICustomUIElement
{
    void OnHover();
    void OnExit();
    void OnClick();
}