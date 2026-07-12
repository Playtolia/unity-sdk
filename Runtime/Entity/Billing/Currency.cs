using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class Currency
    {
        /*
         * @PrimaryKey
           @SerialName("ID")
           val id: String,
           val code: String,
           val name: String
         */
        
        [JsonProperty("ID")]
        public string Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}