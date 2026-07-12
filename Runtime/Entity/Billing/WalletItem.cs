using System;
using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    public class WalletItem
    {
        [JsonProperty("ID")] public string Id { get; set; }

        [JsonProperty("balance")] public string Balance { get; set; }

        [JsonProperty("currency")] public Currency Currency { get; set; }
    }
}