using System;
using Playtolia.Entity.Promotion;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaPromotion : VirtualStateful
    {
        private static PlaytoliaPromotion _instance;

        public static PlaytoliaPromotion Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaPromotion();
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

        public static PromotionState GetState()
        {
            return PlaytoliaPromotionPlatformFunc.GetState();
        }

        public static void ConfigureReview(int minSessions, int minDaysSinceInstall, int maxRequestsPerYear, int cooldownDays)
        {
            PlaytoliaPromotionPlatformFunc.ConfigureReview(minSessions, minDaysSinceInstall, maxRequestsPerYear, cooldownDays);
        }

        public static void RequestReview(bool force = false)
        {
            PlaytoliaPromotionPlatformFunc.RequestReview(force);
        }

        public static void RequestReviewAfterPositiveEvent(bool force = false)
        {
            PlaytoliaPromotionPlatformFunc.RequestReviewAfterPositiveEvent(force);
        }

        public static bool CanRequestReview()
        {
            return PlaytoliaPromotionPlatformFunc.CanRequestReview();
        }

        public static void SetNeverAskAgain(bool value)
        {
            PlaytoliaPromotionPlatformFunc.SetNeverAskAgain(value);
        }
    }
}
