using System.Collections.Generic;
using Newtonsoft.Json;

namespace Playtolia.Entity.Push
{
    public class PushState
    {
        [JsonProperty("preferences")]
        public Dictionary<string, bool> Preferences { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }

        public PushState()
        {
            Preferences = new Dictionary<string, bool>();
            Permission = "NotGranted";
        }
    }
}
