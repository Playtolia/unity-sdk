using Newtonsoft.Json;

namespace Playtolia.Entity.Social
{
    public class Friend
    {
        [JsonProperty("friendsSince")]
        public string FriendsSince { get; set; }
        
        [JsonProperty("friendshipId")]
        public string FriendshipId { get; set; }
        
        [JsonProperty("unreadCount")]
        public string UnreadCount { get; set; }
        
        [JsonProperty("user")]
        public Player User { get; set; }
    }
}

