using System;
using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class PlayerEntitlement
    {
        [JsonProperty("ID")]
        public string Id { get; set; }
        
        [JsonProperty("amount")]
        public long Amount { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("expiresAt")]
        public DateTime ExpiresAt { get; set; }
        
        [JsonProperty("grantID")]
        public string GrantId { get; set; }
        
        [JsonProperty("storeItem")]
        public StoreItem StoreItem { get; set; }
        
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    }
}
