using Newtonsoft.Json;

namespace Playtolia.Entity.Party
{
    public class PartyInvite
    {
        [JsonProperty("inviteID")]
        public string InviteId { get; set; }
        
        [JsonProperty("partyID")]
        public string PartyId { get; set; }
        
        [JsonProperty("inviterID")]
        public string InviterId { get; set; }
        
        [JsonProperty("inviteeID")]
        public string InviteeId { get; set; }
        
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("expiresAt")]
        public string ExpiresAt { get; set; }
    }
}

