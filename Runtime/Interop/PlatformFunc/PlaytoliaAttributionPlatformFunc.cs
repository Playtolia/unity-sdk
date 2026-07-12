using System.Runtime.InteropServices;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaAttributionPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern System.IntPtr Attribution_GetAttributionDataJson();
        [DllImport("__Internal")] private static extern System.IntPtr Attribution_GetInstallReferrerJson();
#else
        private static System.IntPtr Attribution_GetAttributionDataJson() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Attribution_GetInstallReferrerJson() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
#endif

        internal static string GetAttributionDataJson()
        {
            return CallJson(Attribution_GetAttributionDataJson, "getAttributionDataJson");
        }

        internal static string GetInstallReferrerJson()
        {
            return CallJson(Attribution_GetInstallReferrerJson, "getInstallReferrerJson");
        }

        private static string CallJson(System.Func<System.IntPtr> iosFunc, string androidMethod)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr ptr = iosFunc();
                    if (ptr == System.IntPtr.Zero)
                    {
                        return null;
                    }

                    string str = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeHGlobal(ptr);
                    return string.IsNullOrEmpty(str) ? null : str;
                case RuntimePlatform.Android:
                    string androidStr =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Attribution, androidMethod, true);
                    return string.IsNullOrEmpty(androidStr) ? null : androidStr;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}
