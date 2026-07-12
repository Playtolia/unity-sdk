using Newtonsoft.Json;

namespace Playtolia.Entity.Attribution
{
    /// <summary>
    /// Raw Google Play install referrer details (SYSTEM provider, Android only).
    /// </summary>
    public class InstallReferrerInfo
    {
        [JsonProperty("referrerUrl")]
        public string ReferrerUrl { get; set; }

        [JsonProperty("referrerClickTimestampSeconds")]
        public long ReferrerClickTimestampSeconds { get; set; }

        [JsonProperty("installBeginTimestampSeconds")]
        public long InstallBeginTimestampSeconds { get; set; }

        [JsonProperty("googlePlayInstant")]
        public bool GooglePlayInstant { get; set; }
    }
}
