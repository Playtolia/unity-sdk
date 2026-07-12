using Newtonsoft.Json;

namespace Playtolia.Entity.Promotion
{
    public class PromotionState
    {
        [JsonProperty("sessionCount")]
        public int SessionCount { get; set; }

        [JsonProperty("firstSessionTimestamp")]
        public long FirstSessionTimestamp { get; set; }

        [JsonProperty("lastReviewRequestTimestamp")]
        public long LastReviewRequestTimestamp { get; set; }

        [JsonProperty("reviewRequestCount")]
        public int ReviewRequestCount { get; set; }

        [JsonProperty("neverAskAgain")]
        public bool NeverAskAgain { get; set; }

        public PromotionState()
        {
            SessionCount = 0;
            FirstSessionTimestamp = 0;
            LastReviewRequestTimestamp = 0;
            ReviewRequestCount = 0;
            NeverAskAgain = false;
        }
    }
}
