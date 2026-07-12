using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaInterop
    {
        public static void UnsupportedPlatform()
        {
            Debug.LogWarning("PlaytoliaInterop: Unsupported platform call. This method should not be called on this platform.");
        }
    }
}