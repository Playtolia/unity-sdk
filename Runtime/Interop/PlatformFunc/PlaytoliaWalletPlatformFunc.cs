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
    public class PlaytoliaWalletPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Wallet_Refresh();
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_SerializeState();
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_GetCurrencies();
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_GetCurrencyById(string currencyId);
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_GetCurrencyByCode(string code);
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_GetWalletForCurrencyWithId(string currencyId);
        [DllImport("__Internal")] private static extern System.IntPtr Wallet_GetWalletForCurrencyWithCode(string code);
#else
        private static void Wallet_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static System.IntPtr Wallet_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Wallet_GetCurrencies() { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Wallet_GetCurrencyById(string currencyId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Wallet_GetCurrencyByCode(string code) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Wallet_GetWalletForCurrencyWithId(string currencyId) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
        private static System.IntPtr Wallet_GetWalletForCurrencyWithCode(string code) { PlaytoliaInterop.UnsupportedPlatform(); return System.IntPtr.Zero; }
#endif
        
        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Wallet_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static List<WalletItem> GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr statePtr = Wallet_SerializeState();
                    if (statePtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<WalletItem>>(stateString);
                case RuntimePlatform.Android:
                    string stateString2 =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "serializeState", true);
                    if (string.IsNullOrEmpty(stateString2))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetState returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<WalletItem>>(stateString2);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static WalletItem GetWalletForCurrencyWithId(string id)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Wallet_GetWalletForCurrencyWithId(id);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithId returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithId returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<WalletItem>(resultString);
                case RuntimePlatform.Android:
                    string itemString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "getWalletForCurrencyWithId", true, id);
                    if (string.IsNullOrEmpty(itemString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithId returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<WalletItem>(itemString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
        
        internal static WalletItem GetWalletForCurrencyWithCode(string code)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Wallet_GetWalletForCurrencyWithCode(code);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithCode returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithCode returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<WalletItem>(resultString);
                case RuntimePlatform.Android:
                    string itemString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "getWalletForCurrencyWithCode", true, code);
                    if (string.IsNullOrEmpty(itemString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetWalletForCurrencyWithCode returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<WalletItem>(itemString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<Currency> GetCurrencies()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Wallet_GetCurrencies();
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencies returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencies returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<List<Currency>>(resultString);
                case RuntimePlatform.Android:
                    string currenciesString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "getCurrencies", true);
                    if (string.IsNullOrEmpty(currenciesString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencies returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<List<Currency>>(currenciesString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static Currency GetCurrencyById(string id)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Wallet_GetCurrencyById(id);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyById returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyById returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<Currency>(resultString);
                case RuntimePlatform.Android:
                    string currencyString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "getCurrencyById", true, id);
                    if (string.IsNullOrEmpty(currencyString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyById returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<Currency>(currencyString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
        
        internal static Currency GetCurrencyByCode(string code)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    System.IntPtr resultPtr = Wallet_GetCurrencyByCode(code);
                    if (resultPtr == System.IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyByCode returned null or empty value.");
                        return null;
                    }
                    
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    // Free the native memory
                    Marshal.FreeHGlobal(resultPtr);
                    
                    if (string.IsNullOrEmpty(resultString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyByCode returned null or empty value.");
                        return null;
                    }
                    return JsonConvert.DeserializeObject<Currency>(resultString);
                case RuntimePlatform.Android:
                    string currencyString =
                        PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Wallet, "getCurrencyByCode", true, code);
                    if (string.IsNullOrEmpty(currencyString))
                    {
                        Debug.LogWarning("PlaytoliaWalletPlatformFunc: GetCurrencyByCode returned null or empty value.");
                        return null;
                    }

                    return JsonConvert.DeserializeObject<Currency>(currencyString);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }
    }
}