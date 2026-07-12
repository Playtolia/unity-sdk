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
    public class PlaytoliaEntitlementsPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Entitlements_Refresh();
        [DllImport("__Internal")] private static extern System.IntPtr Entitlements_GetAllEntitlements();
        [DllImport("__Internal")] private static extern System.IntPtr Entitlements_GetEntitlementByGrantId(string grantId);
        [DllImport("__Internal")] private static extern System.IntPtr Entitlements_GetEntitlementsByGrantId(string grantId);
        [DllImport("__Internal")] private static extern System.IntPtr Entitlements_GetEntitlementByItemId(string itemId);
#else
        private static void Entitlements_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Entitlements_GetAllEntitlements() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Entitlements_GetEntitlementByGrantId(string grantId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Entitlements_GetEntitlementsByGrantId(string grantId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Entitlements_GetEntitlementByItemId(string itemId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
#endif
        
        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Entitlements_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Entitlements, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static List<PlayerEntitlement> GetAllEntitlements()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Entitlements_GetAllEntitlements();
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetAllEntitlements returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetAllEntitlements returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerEntitlement>>(resultString);
                case RuntimePlatform.Android:
                    string entitlementsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Entitlements, "getAllEntitlements", true);
                    if (string.IsNullOrEmpty(entitlementsString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetAllEntitlements returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerEntitlement>>(entitlementsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static PlayerEntitlement GetEntitlementByGrantId(string grantId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Entitlements_GetEntitlementByGrantId(grantId);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByGrantId returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByGrantId returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<PlayerEntitlement>(resultString);
                case RuntimePlatform.Android:
                    string entitlementString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Entitlements, "getEntitlementByGrantId", true, grantId);
                    if (string.IsNullOrEmpty(entitlementString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByGrantId returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PlayerEntitlement>(entitlementString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<PlayerEntitlement> GetEntitlementsByGrantId(string grantId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Entitlements_GetEntitlementsByGrantId(grantId);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementsByGrantId returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementsByGrantId returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<PlayerEntitlement>>(resultString);
                case RuntimePlatform.Android:
                    string entitlementsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Entitlements, "getEntitlementsByGrantId", true, grantId);
                    if (string.IsNullOrEmpty(entitlementsString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementsByGrantId returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<PlayerEntitlement>>(entitlementsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static PlayerEntitlement GetEntitlementByItemId(string itemId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Entitlements_GetEntitlementByItemId(itemId);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByItemId returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByItemId returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<PlayerEntitlement>(resultString);
                case RuntimePlatform.Android:
                    string entitlementString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Entitlements, "getEntitlementByItemId", true, itemId);
                    if (string.IsNullOrEmpty(entitlementString))
                    {
                        Debug.LogWarning("PlaytoliaEntitlementsPlatformFunc: GetEntitlementByItemId returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<PlayerEntitlement>(entitlementString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}
