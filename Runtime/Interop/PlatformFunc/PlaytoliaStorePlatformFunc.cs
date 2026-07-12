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
    public class PlaytoliaStorePlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Store_Refresh();
        [DllImport("__Internal")] private static extern System.IntPtr Store_SerializeState();
        [DllImport("__Internal")] private static extern System.IntPtr Store_SearchItems(string query);
        [DllImport("__Internal")] private static extern System.IntPtr Store_GetItemsByType(string type);
        [DllImport("__Internal")] private static extern System.IntPtr Store_GetItemById(string itemId);
        [DllImport("__Internal")] private static extern System.IntPtr Store_GetItemBySku(string sku);
        [DllImport("__Internal")] private static extern void Store_BeginPurchaseFlow(string itemId, string callbackKey);
#else
        private static void Store_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Store_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Store_SearchItems(string query) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Store_GetItemsByType(string type) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Store_GetItemById(string itemId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Store_GetItemBySku(string sku) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static void Store_BeginPurchaseFlow(string itemId, string callbackKey) { PlaytoliaInterop.UnsupportedPlatform(); }
#endif
        
        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Store_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static List<StoreItem> GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr statePtr = Store_SerializeState();
                    if (statePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<StoreItem>>(stateString);
                case RuntimePlatform.Android:
                    string androidStateString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetState returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<StoreItem>>(androidStateString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<StoreItem> SearchItems(string query)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Store_SearchItems(query);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: SearchItems returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: SearchItems returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<StoreItem>>(resultString);
                case RuntimePlatform.Android:
                    string itemsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "searchItems", true, query);
                    if (string.IsNullOrEmpty(itemsString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: SearchItems returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<StoreItem>>(itemsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<StoreItem> GetItemsByType(string type)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Store_GetItemsByType(type);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemsByType returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemsByType returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<StoreItem>>(resultString);
                case RuntimePlatform.Android:
                    string itemsString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "getItemsByType", true, type);
                    if (string.IsNullOrEmpty(itemsString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemsByType returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<StoreItem>>(itemsString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static StoreItem GetItemById(string id)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Store_GetItemById(id);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemById returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemById returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<StoreItem>(resultString);
                case RuntimePlatform.Android:
                    string itemString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "getItemById", true, id);
                    if (string.IsNullOrEmpty(itemString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemById returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<StoreItem>(itemString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static StoreItem GetItemBySku(string sku)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Store_GetItemBySku(sku);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemBySku returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemBySku returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<StoreItem>(resultString);
                case RuntimePlatform.Android:
                    string itemString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "getItemBySku", true, sku);
                    if (string.IsNullOrEmpty(itemString))
                    {
                        Debug.LogWarning("PlaytoliaStorePlatformFunc: GetItemBySku returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<StoreItem>(itemString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static void BeginPurchaseFlow(string itemId, Action<string> onSuccess = null, Action<string> onError = null)
        {
            var callbackKey = InteropBridge.Register(onSuccess, onError);

            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Store_BeginPurchaseFlow(itemId, callbackKey);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Store, "beginPurchaseFlow", false, itemId, callbackKey);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}