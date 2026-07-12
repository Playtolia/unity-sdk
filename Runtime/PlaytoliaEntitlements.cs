using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Playtolia.Entity;
using Playtolia.Entity.Billing;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaEntitlements: VirtualStateful
    {
        private static PlaytoliaEntitlements _instance;
        
        public static PlaytoliaEntitlements Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaEntitlements();
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
            PlaytoliaEntitlementsPlatformFunc.Refresh();
        }
        
        public static List<PlayerEntitlement> GetAllEntitlements()
        {
            return PlaytoliaEntitlementsPlatformFunc.GetAllEntitlements();
        }
        
        public static PlayerEntitlement GetEntitlementByGrantId(string grantId)
        {
            return PlaytoliaEntitlementsPlatformFunc.GetEntitlementByGrantId(grantId);
        }
        
        public static List<PlayerEntitlement> GetEntitlementsByGrantId(string grantId)
        {
            return PlaytoliaEntitlementsPlatformFunc.GetEntitlementsByGrantId(grantId);
        }
        
        public static PlayerEntitlement GetEntitlementByItemId(string itemId)
        {
            return PlaytoliaEntitlementsPlatformFunc.GetEntitlementByItemId(itemId);
        }
    }
}