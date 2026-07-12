using Newtonsoft.Json;

namespace Playtolia.Entity
{
    public class AuthState
    {
        
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("exp")]
        public long Expiration { get; set; }
        [JsonProperty("refreshExp")]
        public long RefreshTokenExpiration { get; set; }
    }
}