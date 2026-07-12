using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity;
using Playtolia.Interop.Adapter;
using UnityEngine;
using System;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaAuthPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Auth_PromptLogin(bool dismissable);
        [DllImport("__Internal")] private static extern void Auth_CancelLogin();
        [DllImport("__Internal")] private static extern void Auth_Logout();
        [DllImport("__Internal")] private static extern IntPtr Auth_SerializeState();
        [DllImport("__Internal")] private static extern bool Auth_IsLoggedIn();
        [DllImport("__Internal")] private static extern IntPtr Auth_GetAccessToken();
#else
        private static void Auth_PromptLogin(bool dismissable) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Auth_CancelLogin() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Auth_Logout() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Auth_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static bool Auth_IsLoggedIn() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static IntPtr Auth_GetAccessToken() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
#endif
        
        internal static void PromptLogin(bool dismissable) {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Auth_PromptLogin(dismissable);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Scaffold, "promptLogin", false, dismissable);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
		}
        
        internal static void CancelLogin(bool dismissable) {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Auth_CancelLogin();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Scaffold, "cancelLogin", false, dismissable);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
        
        internal static void Logout() {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Auth_Logout();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Auth, "logout");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static bool IsLoggedIn()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return Auth_IsLoggedIn();
                case RuntimePlatform.Android:
                    string result = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Auth, "isLoggedIn", true);
                    return result == "true";
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static string GetAccessToken()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr tokenPtr = Auth_GetAccessToken();
                    if (tokenPtr == IntPtr.Zero) return string.Empty;
                    string token = Marshal.PtrToStringAnsi(tokenPtr);
                    Marshal.FreeHGlobal(tokenPtr);
                    return token ?? string.Empty;
                case RuntimePlatform.Android:
                    return PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Auth, "getAccessToken", true) ?? string.Empty;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return string.Empty;
            }
        }

        internal static AuthState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr statePtr = Auth_SerializeState();
                    if (statePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaAuthPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaAuthPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<AuthState>(stateString);
                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Auth, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaAuthPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<AuthState>(androidStateString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}