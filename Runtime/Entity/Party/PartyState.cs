using System.Collections.Generic;
using Newtonsoft.Json;

namespace Playtolia.Entity.Party
{
    public class PartyState
    {
        [JsonProperty("currentParty")]
        public Party CurrentParty { get; set; }
        
        [JsonProperty("members")]
        public List<PartyMember> Members { get; set; }
        
        [JsonProperty("matchmakingTicket")]
        public MatchmakingTicket MatchmakingTicket { get; set; }
        
        [JsonProperty("pendingInvites")]
        public List<PartyInvite> PendingInvites { get; set; }
        
        public PartyState()
        {
            CurrentParty = null;
            Members = new List<PartyMember>();
            MatchmakingTicket = null;
            PendingInvites = new List<PartyInvite>();
        }
    }
}

