using System;
using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class PlayerSubscription
    {
        [JsonProperty("ID")]
        public string Id { get; set; }
        
        [JsonProperty("autoRenew")]
        public bool AutoRenew { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }
        
        [JsonProperty("startsAt")]
        public DateTime StartsAt { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("storeItem")]
        public StoreItem StoreItem { get; set; }
        
        public bool IsValid => Status?.ToLower() == "subscribed";
        
        public bool IsActive => Status?.ToLower() == "active";
        
        public bool IsExpired => Status?.ToLower() == "expired";
        
        public bool IsCancelled => Status?.ToLower() == "cancelled";
        
        public string Type => StoreItem.Type;
        
        public bool HasBegun => DateTime.UtcNow >= StartsAt;
    }
}
