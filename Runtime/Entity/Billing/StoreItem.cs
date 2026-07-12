using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class StoreItem
    {
        
        [JsonProperty("ID")]
        public string Id { get; set; }
        [JsonProperty("grants")]
        public Grant[] Grants { get; set; }
        [JsonProperty("itemType")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sku")]
        public string Sku { get; set; }
    }
}