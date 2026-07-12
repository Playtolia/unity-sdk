using System.Collections.Generic;
using Newtonsoft.Json;

namespace Playtolia.Entity.Party
{
    public class PartyMember
    {
        [JsonProperty("playerID")]
        public string PlayerId { get; set; }
        
        [JsonProperty("role")]
        public string Role { get; set; }
        
        [JsonProperty("ready")]
        public bool Ready { get; set; }
        
        [JsonProperty("joinedAt")]
        public string JoinedAt { get; set; }
        
        [JsonProperty("attributes")]
        public Dictionary<string, object> Attributes { get; set; }
    }
}

