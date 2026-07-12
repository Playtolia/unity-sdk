using Newtonsoft.Json;

namespace Playtolia.Entity.Social
{
    public class Player
    {
        [JsonProperty("ID")]
        public string Id { get; set; }
        
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        
        [JsonProperty("playerID")]
        public string PlayerId { get; set; }
        
        [JsonProperty("profileImageUrl")]
        public string Avatar { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}

