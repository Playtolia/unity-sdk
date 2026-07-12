using Newtonsoft.Json;
using Playtolia.Entity.Attribution;

namespace PlaytoliaSDK.Runtime
{
    /// <summary>
    /// Read access to marketing attribution resolved by the SDK.
    ///
    /// Populated when an attribution provider is configured (see Playtolia Settings → Attribution).
    /// With the "System" provider, <see cref="GetInstallReferrer"/> returns the Google Play install
    /// referrer on Android; iOS has no equivalent and returns null.
    /// </summary>
    public class PlaytoliaAttribution
    {
        /// <summary>
        /// Resolved attribution data (parsed UTM parameters), or null if none is available yet.
        /// </summary>
        public static AttributionData GetAttributionData()
        {
            string json = PlaytoliaAttributionPlatformFunc.GetAttributionDataJson();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<AttributionData>(json);
        }

        /// <summary>
        /// Raw Google Play install referrer details (Android, "System" provider), or null.
        /// </summary>
        public static InstallReferrerInfo GetInstallReferrer()
        {
            string json = PlaytoliaAttributionPlatformFunc.GetInstallReferrerJson();
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<InstallReferrerInfo>(json);
        }
    }
}
