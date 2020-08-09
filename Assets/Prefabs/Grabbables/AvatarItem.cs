using UnityEngine;

public class AvatarItem : MonoBehaviour, IGrabbable
{
    [SerializeField] private GameObject model = default;

    public void OnGrab()
    {
        model.SetActive(true);
    }

    public void OnDrop()
    {
        model.SetActive(false);
    }
}