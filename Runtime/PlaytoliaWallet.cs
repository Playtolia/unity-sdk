using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Playtolia.Entity;
using Playtolia.Entity.Billing;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaWallet: VirtualStateful
    {
        private static PlaytoliaWallet _instance;
        
        public static PlaytoliaWallet Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaWallet();
                }
                return _instance;
            }
        }
        
        public static void AddListener(Action listener)
        {
            Instance.AddStateChangedListener(listener);
        }
        
        public static void RemoveListener(Action listener)
        {
            Instance.RemoveStateChangedListener(listener);
        }
        
        public static WalletItem GetWallet(Currency currency)
        {
            return PlaytoliaWalletPlatformFunc.GetWalletForCurrencyWithId(currency.Id);
        }
        
        public static WalletItem GetWalletForCurrencyWithId(string currencyId)
        {
            return PlaytoliaWalletPlatformFunc.GetWalletForCurrencyWithId(currencyId);
        }
        
        public static WalletItem GetWalletForCurrencyWithCode(string currencyCode)
        {
            return PlaytoliaWalletPlatformFunc.GetWalletForCurrencyWithCode(currencyCode);
        }
        
        public static List<Currency> GetCurrencies()
        {
            return PlaytoliaWalletPlatformFunc.GetCurrencies();
        }
        
        public static Currency GetCurrencyById(string id)
        {
            return PlaytoliaWalletPlatformFunc.GetCurrencyById(id);
        }
        
        public static Currency GetCurrencyByCode(string code)
        {
            return PlaytoliaWalletPlatformFunc.GetCurrencyByCode(code);
        }
        
        public static void Refresh()
        {
            PlaytoliaWalletPlatformFunc.Refresh();
        }
        
        public static List<WalletItem> GetWallets()
        {
            return PlaytoliaWalletPlatformFunc.GetState();
        }
    }
}