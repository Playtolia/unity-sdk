using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Push;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaNotificationsPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Notifications_Refresh();
        [DllImport("__Internal")] private static extern IntPtr Notifications_SerializeState();
        [DllImport("__Internal")] private static extern void Notifications_RegisterPushToken(string token);
        [DllImport("__Internal")] private static extern void Notifications_RequestPushPermission();
        [DllImport("__Internal")] private static extern void Notifications_OpenNotificationSettings();
        [DllImport("__Internal")] private static extern IntPtr Notifications_GetPermission();
        [DllImport("__Internal")] private static extern void Notifications_SendTestPush(string title, string body, string data);
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Notifications_Refresh();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Notifications_SerializeState();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Notifications_RegisterPushToken(byte[] token);
        private static void Notifications_RegisterPushToken(string token) { Notifications_RegisterPushToken(NativeStrings.ToUtf8(token)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Notifications_RequestPushPermission();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Notifications_OpenNotificationSettings();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Notifications_GetPermission();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Notifications_SendTestPush(byte[] title, byte[] body, byte[] data);
        private static void Notifications_SendTestPush(string title, string body, string data) { Notifications_SendTestPush(NativeStrings.ToUtf8(title), NativeStrings.ToUtf8(body), NativeStrings.ToUtf8(data)); }
#else
        private static void Notifications_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Notifications_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static void Notifications_RegisterPushToken(string token) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Notifications_RequestPushPermission() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Notifications_OpenNotificationSettings() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Notifications_GetPermission() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static void Notifications_SendTestPush(string title, string body, string data) { PlaytoliaInterop.UnsupportedPlatform(); }
#endif

        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Notifications_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static PushState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr statePtr = Notifications_SerializeState();
                    if (statePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaNotificationsPlatformFunc: GetState returned null or empty value.");
                        return new PushState();
                    }

                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    Marshal.FreeHGlobal(statePtr);

                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaNotificationsPlatformFunc: GetState returned null or empty value.");
                        return new PushState();
                    }
                    return JsonConvert.DeserializeObject<PushState>(stateString);

                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winStatePtr = Notifications_SerializeState();
                    if (winStatePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaNotificationsPlatformFunc: GetState returned null or empty value.");
                        return new PushState();
                    }

                    string winStateString = NativeStrings.TakeUtf8(winStatePtr);

                    if (string.IsNullOrEmpty(winStateString))
                    {
                        Debug.LogWarning("PlaytoliaNotificationsPlatformFunc: GetState returned null or empty value.");
                        return new PushState();
                    }
                    return JsonConvert.DeserializeObject<PushState>(winStateString);
                }
                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaNotificationsPlatformFunc: GetState returned null or empty value.");
                        return new PushState();
                    }
                    return JsonConvert.DeserializeObject<PushState>(androidStateString);

                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new PushState();
            }
        }

        internal static void RegisterPushToken(string token)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Notifications_RegisterPushToken(token);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "registerPushToken", false, token);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void RequestPushPermission()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Notifications_RequestPushPermission();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "requestPushPermission");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void OpenNotificationSettings()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Notifications_OpenNotificationSettings();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "openNotificationSettings");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static string GetPermission()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr permPtr = Notifications_GetPermission();
                    if (permPtr == IntPtr.Zero) return "NotGranted";
                    string permString = Marshal.PtrToStringAnsi(permPtr);
                    Marshal.FreeHGlobal(permPtr);
                    if (string.IsNullOrEmpty(permString)) return "NotGranted";
                    return JsonConvert.DeserializeObject<string>(permString) ?? "NotGranted";

                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winPermPtr = Notifications_GetPermission();
                    if (winPermPtr == IntPtr.Zero) return "NotGranted";
                    string winPermString = NativeStrings.TakeUtf8(winPermPtr);
                    if (string.IsNullOrEmpty(winPermString)) return "NotGranted";
                    return JsonConvert.DeserializeObject<string>(winPermString) ?? "NotGranted";
                }
                case RuntimePlatform.Android:
                    string androidPermString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "getPermission", true);
                    if (string.IsNullOrEmpty(androidPermString)) return "NotGranted";
                    return JsonConvert.DeserializeObject<string>(androidPermString) ?? "NotGranted";

                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return "NotGranted";
            }
        }
        internal static void SendTestPush(string title, string body, string data = null)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Notifications_SendTestPush(title, body, data);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Notifications, "sendTestPush", false, title, body, data ?? "");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}
