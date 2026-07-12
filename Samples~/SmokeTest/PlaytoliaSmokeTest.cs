using UnityEngine;
using PlaytoliaSDK.Runtime;

namespace PlaytoliaSDK.Samples
{
    /// <summary>
    /// Minimal smoke test for the Playtolia SDK. Attach to a GameObject in an
    /// otherwise empty scene (alongside a PlaytoliaGameObject), build for Android
    /// or iOS, and watch the log.
    ///
    /// It issues a couple of read-only compat calls to prove the native bridge is
    /// wired up (Kotlin -> ObjC/JNI -> C#). It does not require a signed-in user.
    ///
    /// CI uses the compilation of this sample as the package smoke gate; on-device
    /// runs use it as the manual verification entry point.
    /// </summary>
    public class PlaytoliaSmokeTest : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log($"[PlaytoliaSmokeTest] Running on {Application.platform}");

            try
            {
                // Read-only compat calls that round-trip through the native bridge.
                bool loggedIn = PlaytoliaAuth.IsLoggedIn();
                Debug.Log($"[PlaytoliaSmokeTest] IsLoggedIn: {loggedIn}");

                string token = PlaytoliaAuth.GetAccessToken();
                Debug.Log($"[PlaytoliaSmokeTest] Access token present: {!string.IsNullOrEmpty(token)}");

                Debug.Log("[PlaytoliaSmokeTest] PASS — native bridge reachable.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[PlaytoliaSmokeTest] FAIL — {e.GetType().Name}: {e.Message}");
            }
        }
    }
}
