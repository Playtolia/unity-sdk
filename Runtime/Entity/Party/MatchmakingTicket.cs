using Newtonsoft.Json;

namespace Playtolia.Entity.Party
{
    public class MatchmakingTicket
    {
        [JsonProperty("ticketID")]
        public string TicketId { get; set; }
        
        [JsonProperty("queueName")]
        public string QueueName { get; set; }
        
        [JsonProperty("enqueuedAt")]
        public string EnqueuedAt { get; set; }
    }
}

