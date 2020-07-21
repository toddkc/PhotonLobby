using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

/// <summary>
/// testing how to get PUN VOICE to work on quest...
/// </summary>

public class AudioPermissions : MonoBehaviour
{
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }
}