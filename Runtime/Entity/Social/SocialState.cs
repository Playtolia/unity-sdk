using System.Collections.Generic;
using Newtonsoft.Json;

namespace Playtolia.Entity.Social
{
    public class SocialState
    {
        [JsonProperty("friends")]
        public List<Friend> Friends { get; set; }
        
        [JsonProperty("incomingFriendRequests")]
        public List<FriendRequest> IncomingFriendRequests { get; set; }
        
        [JsonProperty("outgoingFriendRequests")]
        public List<FriendRequest> OutgoingFriendRequests { get; set; }
        
        public SocialState()
        {
            Friends = new List<Friend>();
            IncomingFriendRequests = new List<FriendRequest>();
            OutgoingFriendRequests = new List<FriendRequest>();
        }
    }
}

