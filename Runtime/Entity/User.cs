using Newtonsoft.Json;

namespace Playtolia.Entity
{
    public class User
    {
        
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("ID")]
        public string Uid { get; set; }
        [JsonProperty("playerID")]
        public string PlayerId { get; set; }
        [JsonProperty("lang")]
        public string Lang { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        [JsonProperty("profileImageUrl")]
        public string Avatar { get; set; }
    }
}