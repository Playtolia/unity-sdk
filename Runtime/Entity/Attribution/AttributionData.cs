using Newtonsoft.Json;

namespace Playtolia.Entity.Attribution
{
    /// <summary>
    /// Attribution data resolved by the active provider (e.g. parsed install-referrer UTM params).
    /// </summary>
    public class AttributionData
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("campaign")]
        public string Campaign { get; set; }

        [JsonProperty("adGroup")]
        public string AdGroup { get; set; }

        [JsonProperty("adCreative")]
        public string AdCreative { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("term")]
        public string Term { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
