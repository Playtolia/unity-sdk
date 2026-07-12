using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Playtolia.Entity.Billing
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreItemType
    {
        [EnumMember(Value = "CONSUMABLE")]
        Consumable,
        
        [EnumMember(Value = "NON_CONSUMABLE")]
        NonConsumable,
        
        [EnumMember(Value = "SUBSCRIPTION")]
        Subscription
    }
}
