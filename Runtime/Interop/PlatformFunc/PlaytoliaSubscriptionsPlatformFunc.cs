using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity;
using Playtolia.Entity.Billing;
using Playtolia.Interop.Adapter;
using UnityEngine;
using System;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSubscriptionsPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Subscriptions_Refresh();
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_SerializeState();
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetActiveSubscriptions();
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetAllSubscriptions();
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetSubscriptionById(string subscriptionId);
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetSubscriptionByItemId(string itemId);
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetSubscriptionByItemSku(string itemSku);
        [DllImport("__Internal")] private static extern System.IntPtr Subscriptions_GetSubscriptionsByType(string type);
        [DllImport("__Internal")] private static extern bool Subscriptions_HasActiveSubscription();
        [DllImport("__Internal")] private static extern bool Subscriptions_HasActiveSubscriptionOfType(string type);
#else
        private static void Subscriptions_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Subscriptions_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetActiveSubscriptions() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetAllSubscriptions() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetSubscriptionById(string subscriptionId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetSubscriptionByItemId(string itemId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetSubscriptionByItemSku(string itemSku) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Subscriptions_GetSubscriptionsByType(string type) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static bool Subscriptions_HasActiveSubscription() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static bool Subscriptions_HasActiveSubscriptionOfType(string type) { PlaytoliaInterop.UnsupportedPlatform(); return false; }
#endif
        
        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Subscriptions_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static List<PlayerSubscription> GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr statePtr = Subscriptions_SerializeState();
                    if (statePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(stateString);
                case RuntimePlatform.Android:
                    string stateString2 =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "serializeState", true);
                    if (string.IsNullOrEmpty(stateString2))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(stateString2);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static PlayerSubscription GetSubscriptionById(string id)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetSubscriptionById(id);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionById returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionById returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<PlayerSubscription>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getSubscriptionById", true, id);
                    if (string.IsNullOrEmpty(subscriptionString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionById returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PlayerSubscription>(subscriptionString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<PlayerSubscription> GetActiveSubscriptions()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetActiveSubscriptions();
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetActiveSubscriptions returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetActiveSubscriptions returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getActiveSubscriptions", true);
                    if (string.IsNullOrEmpty(subscriptionsString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetActiveSubscriptions returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(subscriptionsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<PlayerSubscription> GetSubscriptionsByType(string type)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetSubscriptionsByType(type);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionsByType returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionsByType returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getSubscriptionsByType", true, type);
                    if (string.IsNullOrEmpty(subscriptionsString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionsByType returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(subscriptionsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static bool HasActiveSubscription()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return Subscriptions_HasActiveSubscription();
                case RuntimePlatform.Android:
                    string resultString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "hasActiveSubscription", true);
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: HasActiveSubscription returned null or empty value.");
                        return false;
                    }

                    return bool.Parse(resultString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static bool HasActiveSubscriptionOfType(string type)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return Subscriptions_HasActiveSubscriptionOfType(type);
                case RuntimePlatform.Android:
                    string resultString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "hasActiveSubscriptionOfType", true, type);
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: HasActiveSubscriptionOfType returned null or empty value.");
                        return false;
                    }

                    return bool.Parse(resultString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static List<PlayerSubscription> GetAllSubscriptions()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetAllSubscriptions();
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetAllSubscriptions returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetAllSubscriptions returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getAllSubscriptions", true);
                    if (string.IsNullOrEmpty(subscriptionsString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetAllSubscriptions returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerSubscription>>(subscriptionsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static PlayerSubscription GetSubscriptionByItemId(string itemId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetSubscriptionByItemId(itemId);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemId returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemId returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<PlayerSubscription>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getSubscriptionByItemId", true, itemId);
                    if (string.IsNullOrEmpty(subscriptionString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemId returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PlayerSubscription>(subscriptionString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static PlayerSubscription GetSubscriptionByItemSku(string itemSku)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Subscriptions_GetSubscriptionByItemSku(itemSku);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemSku returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemSku returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<PlayerSubscription>(resultString);
                case RuntimePlatform.Android:
                    string subscriptionString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Subscriptions, "getSubscriptionByItemSku", true, itemSku);
                    if (string.IsNullOrEmpty(subscriptionString))
                    {
                        Debug.LogWarning("PlaytoliaSubscriptionsPlatformFunc: GetSubscriptionByItemSku returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PlayerSubscription>(subscriptionString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}
