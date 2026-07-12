using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class Grant
    {
        /*
         *     @PrimaryKey
           @SerialName("ID")
           val id: String,
           val currency: Currency,
           val grantAmount: Long,
           val grantID: String,
           val grantType: String
         */
        
        [JsonProperty("ID")]
        public string Id { get; set; }
        [JsonProperty("currency")]
        public Currency Currency { get; set; }
        [JsonProperty("grantAmount")]
        public long GrantAmount { get; set; }
        [JsonProperty("grantID")]
        public string GrantId { get; set; }
        [JsonProperty("grantType")]
        public string GrantType { get; set; }
    }
}