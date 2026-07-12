using UnityEngine;

namespace Playtolia.Interop.Adapter
{
    public class PlaytoliaAndroidInterop
    {
        public static string ComponentCall(InteropComponent component, string method, bool hasOutput = false, params object[] args)
        {
            try
            {
                AndroidJavaObject interopObject = GetComponentInteropObject(component);
                
                if (interopObject == null)
                {
                    UnityEngine.Debug.LogError("PlaytoliaAndroidInterop: Interop object for component " + component + " is null.");
                    return null;
                }

                if (hasOutput)
                {
                    return interopObject.Call<string>(method, args);
                }
                interopObject.Call(method, args);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("PlaytoliaAndroidInterop: Failed to call " + method + " on " + component + ": " + e.Message);
            }
            return null;
        }

        private static AndroidJavaObject GetComponentInteropObject(InteropComponent component)
        {
            AndroidJavaClass playtoliaClass = new AndroidJavaClass("com.playtolia.sdk." + GetComponentPackageComplement(component));

            return playtoliaClass.GetStatic<AndroidJavaObject>("INSTANCE");
        }
        
        private static string GetComponentPackageComplement(InteropComponent component)
        {
            switch (component)
            {
                case InteropComponent.Core:
                    return "core.PlaytoliaSDK";
                case InteropComponent.Auth:
                    return "core.auth.AuthCompat";
                case InteropComponent.Session:
                    return "core.session.SessionCompat";
                case InteropComponent.Store:
                    return "core.billing.StoreCompat";
                case InteropComponent.Wallet:
                    return "core.billing.WalletCompat";
                case InteropComponent.Entitlements:
                    return "core.billing.EntitlementsCompat";
                case InteropComponent.Subscriptions:
                    return "core.billing.SubscriptionsCompat";
                case InteropComponent.Scaffold:
                    return "ui.scaffold.ScaffoldStateful";
                case InteropComponent.Social:
                    return "core.social.SocialCompat";
                case InteropComponent.Party:
                    return "core.party.PartyCompat";
                case InteropComponent.Notifications:
                    return "core.push.NotificationsCompat";
                case InteropComponent.Promotion:
                    return "core.promotion.PromotionCompat";
                case InteropComponent.Analytics:
                    return "core.analytics.AnalyticsCompat";
                case InteropComponent.Attribution:
                    return "core.attribution.AttributionCompat";
                default:
                    throw new System.ArgumentException("Unknown InteropComponent: " + component);
            }
        }
    }
    
    public enum InteropComponent
    {
        Core,
        Auth,
        Session,
        Scaffold,
        Store,
        Wallet,
        Entitlements,
        Subscriptions,
        Social,
        Party,
        Notifications,
        Promotion,
        Analytics,
        Attribution
    }
}