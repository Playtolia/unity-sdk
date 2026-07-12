using System;
using System.Collections.Generic;
using Playtolia.Entity.Party;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaParty : VirtualStateful
    {
        private static PlaytoliaParty _instance;
        
        public static PlaytoliaParty Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaParty();
                }
                return _instance;
            }
        }
        
        public static void AddListener(Action listener)
        {
            Instance.AddStateChangedListener(listener);
        }
        
        public static void RemoveListener(Action listener)
        {
            Instance.RemoveStateChangedListener(listener);
        }
        
        public static void Refresh()
        {
            PlaytoliaPartyPlatformFunc.Refresh();
        }
        
        public static PartyState GetState()
        {
            return PlaytoliaPartyPlatformFunc.GetState();
        }
        
        public static Party GetCurrentParty()
        {
            return PlaytoliaPartyPlatformFunc.GetCurrentParty();
        }
        
        public static List<PartyMember> GetMembers()
        {
            return PlaytoliaPartyPlatformFunc.GetMembers();
        }
        
        public static bool IsLeader()
        {
            return PlaytoliaPartyPlatformFunc.IsLeader();
        }
        
        public static bool IsInParty()
        {
            return PlaytoliaPartyPlatformFunc.IsInParty();
        }
        
        public static bool IsMatchmaking()
        {
            return PlaytoliaPartyPlatformFunc.IsMatchmaking();
        }
        
        public static MatchmakingTicket GetMatchmakingTicket()
        {
            return PlaytoliaPartyPlatformFunc.GetMatchmakingTicket();
        }
        
        public static void CreateParty(int maxSize = 4, bool generateJoinCode = true)
        {
            PlaytoliaPartyPlatformFunc.CreateParty(maxSize, generateJoinCode);
        }
        
        public static void JoinByCode(string joinCode)
        {
            PlaytoliaPartyPlatformFunc.JoinByCode(joinCode);
        }
        
        public static void LeaveParty()
        {
            PlaytoliaPartyPlatformFunc.LeaveParty();
        }
        
        public static void KickMember(string memberId)
        {
            PlaytoliaPartyPlatformFunc.KickMember(memberId);
        }
        
        public static void TransferLeadership(string newLeaderId)
        {
            PlaytoliaPartyPlatformFunc.TransferLeadership(newLeaderId);
        }
        
        public static void SetReady(bool ready)
        {
            PlaytoliaPartyPlatformFunc.SetReady(ready);
        }
        
        public static void InvitePlayer(string playerId)
        {
            PlaytoliaPartyPlatformFunc.InvitePlayer(playerId);
        }
        
        public static void AcceptInvite(string inviteId)
        {
            PlaytoliaPartyPlatformFunc.AcceptInvite(inviteId);
        }
        
        public static void StartMatchmaking(string queueName)
        {
            PlaytoliaPartyPlatformFunc.StartMatchmaking(queueName);
        }
        
        public static void CancelMatchmaking()
        {
            PlaytoliaPartyPlatformFunc.CancelMatchmaking();
        }
    }
}

