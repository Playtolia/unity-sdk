using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Analytics;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaAnalyticsPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern System.IntPtr Analytics_SerializeState();
        [DllImport("__Internal")] private static extern void Analytics_Track(string eventName, string propertiesJson);
        [DllImport("__Internal")] private static extern System.IntPtr Analytics_GetDeviceId();
        [DllImport("__Internal")] private static extern System.IntPtr Analytics_GetSessionId();
        [DllImport("__Internal")] private static extern void Analytics_Flush();
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern System.IntPtr Analytics_SerializeState();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Analytics_Track(byte[] eventName, byte[] propertiesJson);
        private static void Analytics_Track(string eventName, string propertiesJson) { Analytics_Track(NativeStrings.ToUtf8(eventName), NativeStrings.ToUtf8(propertiesJson)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern System.IntPtr Analytics_GetDeviceId();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern System.IntPtr Analytics_GetSessionId();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Analytics_Flush();
#else
        private static System.IntPtr Analytics_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static void Analytics_Track(string eventName, string propertiesJson) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Analytics_GetDeviceId() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Analytics_GetSessionId() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static void Analytics_Flush() { PlaytoliaInterop.UnsupportedPlatform(); }
#endif

        internal static AnalyticsState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr statePtr = Analytics_SerializeState();
                    if (statePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaAnalyticsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }

                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    Marshal.FreeHGlobal(statePtr);

                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaAnalyticsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<AnalyticsState>(stateString);
                case RuntimePlatform.WindowsPlayer:
                {
                    System.IntPtr winStatePtr = Analytics_SerializeState();
                    if (winStatePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaAnalyticsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }

                    string winStateString = NativeStrings.TakeUtf8(winStatePtr);

                    if (string.IsNullOrEmpty(winStateString))
                    {
                        Debug.LogWarning("PlaytoliaAnalyticsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<AnalyticsState>(winStateString);
                }
                case RuntimePlatform.Android:
                    string androidStateString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Analytics, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaAnalyticsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<AnalyticsState>(androidStateString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static void Track(string eventName, string propertiesJson)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Analytics_Track(eventName, propertiesJson);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Analytics, "track", false, eventName, propertiesJson ?? "");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static string GetDeviceId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr ptr = Analytics_GetDeviceId();
                    if (ptr == System.IntPtr.Zero) return null;
                    string deviceId = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeHGlobal(ptr);
                    return deviceId;
                case RuntimePlatform.WindowsPlayer:
                {
                    System.IntPtr winPtr = Analytics_GetDeviceId();
                    if (winPtr == System.IntPtr.Zero) return null;
                    string winDeviceId = NativeStrings.TakeUtf8(winPtr);
                    return winDeviceId;
                }
                case RuntimePlatform.Android:
                    return PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Analytics, "getDeviceId", true);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static string GetSessionId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr ptr = Analytics_GetSessionId();
                    if (ptr == System.IntPtr.Zero) return null;
                    string sessionId = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeHGlobal(ptr);
                    return sessionId;
                case RuntimePlatform.WindowsPlayer:
                {
                    System.IntPtr winPtr = Analytics_GetSessionId();
                    if (winPtr == System.IntPtr.Zero) return null;
                    string winSessionId = NativeStrings.TakeUtf8(winPtr);
                    return winSessionId;
                }
                case RuntimePlatform.Android:
                    return PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Analytics, "getSessionId", true);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static void Flush()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Analytics_Flush();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Analytics, "flush");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}
