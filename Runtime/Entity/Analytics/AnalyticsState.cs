using Newtonsoft.Json;

namespace Playtolia.Entity.Analytics
{
    public class AnalyticsState
    {
        [JsonProperty("initialized")]
        public bool Initialized { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        public AnalyticsState()
        {
            Initialized = false;
            UserId = null;
            DeviceId = null;
            SessionId = null;
        }
    }
}
