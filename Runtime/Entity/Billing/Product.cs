using Newtonsoft.Json;

namespace Playtolia.Entity.Billing
{
    /// <summary>
    /// A sellable product in the game's catalog.
    ///
    /// Three identifiers travel with every product, and they are not interchangeable:
    /// <see cref="Id"/> is the UUID Playtolia generates, <see cref="Code"/> is the identifier you
    /// choose when creating the product (fixed afterwards, and the one to hardcode in game code),
    /// and <see cref="Sku"/> is the identifier on the platform store the product is sold through.
    /// </summary>
    public class Product
    {
        /// <summary>Playtolia-generated UUID. Stable, but opaque — look products up by <see cref="Code"/> instead.</summary>
        [JsonProperty("ID")]
        public string Id { get; set; }

        /// <summary>The code you assigned at creation, for example "gold_500". Identical on every platform.</summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("grants")]
        public Grant[] Grants { get; set; }

        [JsonProperty("itemType")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Platform store product identifier. Only meaningful on the store it is sold through.</summary>
        [JsonProperty("sku")]
        public string Sku { get; set; }
    }
}
