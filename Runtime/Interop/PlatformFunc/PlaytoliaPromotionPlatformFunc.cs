using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Promotion;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaPromotionPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern IntPtr Promotion_SerializeState();
        [DllImport("__Internal")] private static extern void Promotion_ConfigureReview(int minSessions, int minDaysSinceInstall, int maxRequestsPerYear, int cooldownDays);
        [DllImport("__Internal")] private static extern void Promotion_RequestReview(bool force);
        [DllImport("__Internal")] private static extern void Promotion_RequestReviewAfterPositiveEvent(bool force);
        [DllImport("__Internal")] private static extern bool Promotion_CanRequestReview();
        [DllImport("__Internal")] private static extern void Promotion_SetNeverAskAgain(bool value);
#else
        private static IntPtr Promotion_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static void Promotion_ConfigureReview(int minSessions, int minDaysSinceInstall, int maxRequestsPerYear, int cooldownDays) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Promotion_RequestReview(bool force) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Promotion_RequestReviewAfterPositiveEvent(bool force) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static bool Promotion_CanRequestReview() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static void Promotion_SetNeverAskAgain(bool value) { PlaytoliaInterop.UnsupportedPlatform(); }
#endif

        internal static PromotionState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr statePtr = Promotion_SerializeState();
                    if (statePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaPromotionPlatformFunc: GetState returned null or empty value.");
                        return new PromotionState();
                    }

                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    Marshal.FreeHGlobal(statePtr);

                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaPromotionPlatformFunc: GetState returned null or empty value.");
                        return new PromotionState();
                    }
                    return JsonConvert.DeserializeObject<PromotionState>(stateString);

                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaPromotionPlatformFunc: GetState returned null or empty value.");
                        return new PromotionState();
                    }
                    return JsonConvert.DeserializeObject<PromotionState>(androidStateString);

                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new PromotionState();
            }
        }

        internal static void ConfigureReview(int minSessions, int minDaysSinceInstall, int maxRequestsPerYear, int cooldownDays)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Promotion_ConfigureReview(minSessions, minDaysSinceInstall, maxRequestsPerYear, cooldownDays);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "configureReview", false, minSessions, minDaysSinceInstall, maxRequestsPerYear, cooldownDays);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void RequestReview(bool force = false)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Promotion_RequestReview(force);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "requestReview", false, force);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void RequestReviewAfterPositiveEvent(bool force = false)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Promotion_RequestReviewAfterPositiveEvent(force);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "requestReviewAfterPositiveEvent", false, force);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static bool CanRequestReview()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return Promotion_CanRequestReview();
                case RuntimePlatform.Android:
                    string result = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "canRequestReview", true);
                    return result != null && result.Equals("true", StringComparison.OrdinalIgnoreCase);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static void SetNeverAskAgain(bool value)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Promotion_SetNeverAskAgain(value);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Promotion, "setNeverAskAgain", false, value);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}
