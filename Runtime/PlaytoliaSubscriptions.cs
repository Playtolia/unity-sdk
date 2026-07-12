using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Playtolia.Entity;
using Playtolia.Entity.Billing;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSubscriptions: VirtualStateful
    {
        private static PlaytoliaSubscriptions _instance;
        
        public static PlaytoliaSubscriptions Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaSubscriptions();
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
        
        public static void Refresh()
        {
            PlaytoliaSubscriptionsPlatformFunc.Refresh();
        }
        
        public static List<PlayerSubscription> GetAllSubscriptions()
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetAllSubscriptions();
        }
        
        public static PlayerSubscription GetSubscriptionById(string id)
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetSubscriptionById(id);
        }
        
        public static List<PlayerSubscription> GetActiveSubscriptions()
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetActiveSubscriptions();
        }
        
        public static List<PlayerSubscription> GetSubscriptionsByType(string type)
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetSubscriptionsByType(type);
        }
        
        public static bool HasActiveSubscription()
        {
            return PlaytoliaSubscriptionsPlatformFunc.HasActiveSubscription();
        }
        
        public static bool HasActiveSubscriptionOfType(string type)
        {
            return PlaytoliaSubscriptionsPlatformFunc.HasActiveSubscriptionOfType(type);
        }
        
        public static PlayerSubscription GetSubscriptionByItemId(string itemId)
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetSubscriptionByItemId(itemId);
        }
        
        public static PlayerSubscription GetSubscriptionByItemSku(string itemSku)
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetSubscriptionByItemSku(itemSku);
        }
        
        public static PlayerSubscription GetSubscriptionByItem(StoreItem item)
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetSubscriptionByItemId(item.Id);
        }
        
        public static List<PlayerSubscription> GetState()
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetState();
        }
        
        public static List<PlayerSubscription> GetSubscriptions()
        {
            return PlaytoliaSubscriptionsPlatformFunc.GetState();
        }
    }
}
