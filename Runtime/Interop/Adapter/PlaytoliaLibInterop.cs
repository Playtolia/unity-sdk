using System.Runtime.InteropServices;
using PlaytoliaSDK.Runtime.Common.Core;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaLibInterop
    {
        public static void NotifyComponentStateChanges(string component)
        {
            PlaytoliaLibInterop.GetVirtualStatefulForLibSimpleName(component)?.NotifyStateChanged();
        }
        
        public static VirtualStateful GetVirtualStatefulForLibSimpleName(string libSimpleName)
        {
            switch (libSimpleName)
            {
                case "PlaytoliaAuth":
                    return PlaytoliaAuth.Instance;
                case "PlaytoliaSession":
                    return PlaytoliaSession.Instance;
                case "PlaytoliaStore":
                    return PlaytoliaStore.Instance;
                case "PlaytoliaWallet":
                    return PlaytoliaWallet.Instance;
                case "PlaytoliaSocial":
                    return PlaytoliaSocial.Instance;
                case "PlaytoliaParty":
                    return PlaytoliaParty.Instance;
                case "PlaytoliaNotifications":
                    return PlaytoliaNotifications.Instance;
                case "PlaytoliaPromotion":
                    return PlaytoliaPromotion.Instance;
                case "PlaytoliaAnalytics":
                    return PlaytoliaAnalytics.Instance;
                default:
                    Debug.LogWarning($"PlaytoliaLibInterop: Unsupported libSimpleName '{libSimpleName}'. Returning null.");
                    return null;
            }
        }
    }
}