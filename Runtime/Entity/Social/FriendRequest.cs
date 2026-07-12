using Newtonsoft.Json;

namespace Playtolia.Entity.Social
{
    public class FriendRequest
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("recipientID")]
        public string RecipientId { get; set; }
        
        [JsonProperty("requestID")]
        public string RequestId { get; set; }
        
        [JsonProperty("sender")]
        public Player Sender { get; set; }
        
        [JsonProperty("senderID")]
        public string SenderId { get; set; }
        
        [JsonProperty("recipient")]
        public Player Recipient { get; set; }
    }
}

