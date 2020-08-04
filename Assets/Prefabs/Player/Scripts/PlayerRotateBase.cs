using UnityEngine;

public class PlayerRotateBase : MonoBehaviour
{
    protected Transform thisTransform;

    protected virtual void Awake()
    {
        thisTransform = transform;
    }

    public virtual void Rotate(Quaternion rotation)
    {
        thisTransform.rotation = rotation * thisTransform.rotation;
    }
}