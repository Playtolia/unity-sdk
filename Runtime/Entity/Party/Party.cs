using Newtonsoft.Json;

namespace Playtolia.Entity.Party
{
    public class Party
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("gameID")]
        public string GameId { get; set; }
        
        [JsonProperty("leaderID")]
        public string LeaderId { get; set; }
        
        [JsonProperty("maxSize")]
        public int MaxSize { get; set; }
        
        [JsonProperty("joinCode")]
        public string JoinCode { get; set; }
        
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
    }
}

