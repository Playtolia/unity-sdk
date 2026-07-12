using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class PurchaseReceipt
    {
        [JsonProperty("item")]
        public StoreItem Item { get; set; }

        [JsonProperty("receiptId")]
        public string ReceiptId { get; set; }

        [JsonProperty("store")]
        public string Store { get; set; }
    }
}
