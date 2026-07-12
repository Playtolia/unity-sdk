using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity;
using Playtolia.Interop.Adapter;
using UnityEngine;
using System;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSessionPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Session_Refresh();
        [DllImport("__Internal")] private static extern System.IntPtr Session_SerializeState();
        [DllImport("__Internal")] private static extern void Session_UpdateUsername(string username);
        [DllImport("__Internal")] private static extern void Session_UpdateDisplayName(string displayName);
        [DllImport("__Internal")] private static extern void Session_UpdatePassword(string oldPassword, string newPassword);
#else
        private static void Session_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Session_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static void Session_UpdateUsername(string username) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Session_UpdateDisplayName(string displayName) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Session_UpdatePassword(string oldPassword, string newPassword) { PlaytoliaInterop.UnsupportedPlatform(); }
#endif
        
        internal static void Refresh() {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Session_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Session, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void UpdateUsername(string username)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Session_UpdateUsername(username);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Session, "updateUsername", false, username);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void UpdateDisplayName(string displayName)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Session_UpdateDisplayName(displayName);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Session, "updateDisplayName", false, displayName);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void UpdatePassword(string oldPassword, string newPassword)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Session_UpdatePassword(oldPassword, newPassword);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Session, "updatePassword", false, oldPassword, newPassword);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static User GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr statePtr = Session_SerializeState();
                    if (statePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSessionPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaSessionPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<User>(stateString);
                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Session, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaSessionPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<User>(androidStateString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}