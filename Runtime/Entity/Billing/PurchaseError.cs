using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class PurchaseError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("receiptId")]
        public string ReceiptId { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("itemSku")]
        public string ItemSku { get; set; }
    }
}
