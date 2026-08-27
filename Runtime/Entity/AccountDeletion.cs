using Newtonsoft.Json;

namespace Playtolia.Entity
{
    /// <summary>
    /// A pending account deletion, as scheduled by the backend.
    ///
    /// Deletion is deferred, not immediate: the account stays intact until
    /// <see cref="DeletionScheduledAt"/>, and the player cancels it simply by logging in again
    /// before then.
    /// </summary>
    public class AccountDeletion
    {
        /// <summary>ISO timestamp of when the account is scheduled to be deleted.</summary>
        [JsonProperty("deletionScheduledAt")]
        public string DeletionScheduledAt { get; set; }
    }
}
