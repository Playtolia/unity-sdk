using System;
using System.Collections.Generic;
using Playtolia.Entity.Social;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSocial : VirtualStateful
    {
        private static PlaytoliaSocial _instance;
        
        public static PlaytoliaSocial Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaSocial();
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
            PlaytoliaSocialPlatformFunc.Refresh();
        }
        
        public static SocialState GetState()
        {
            return PlaytoliaSocialPlatformFunc.GetState();
        }
        
        public static List<Friend> GetFriends()
        {
            return PlaytoliaSocialPlatformFunc.GetFriends();
        }
        
        public static List<FriendRequest> GetIncomingFriendRequests()
        {
            return PlaytoliaSocialPlatformFunc.GetIncomingFriendRequests();
        }
        
        public static List<FriendRequest> GetOutgoingFriendRequests()
        {
            return PlaytoliaSocialPlatformFunc.GetOutgoingFriendRequests();
        }
        
        public static void SendFriendRequest(string playerId)
        {
            PlaytoliaSocialPlatformFunc.SendFriendRequest(playerId);
        }
        
        public static void AcceptFriendRequest(string requestId)
        {
            PlaytoliaSocialPlatformFunc.AcceptFriendRequest(requestId);
        }
        
        public static void RejectFriendRequest(string requestId)
        {
            PlaytoliaSocialPlatformFunc.RejectFriendRequest(requestId);
        }
        
        public static void CancelFriendRequest(string requestId)
        {
            PlaytoliaSocialPlatformFunc.CancelFriendRequest(requestId);
        }
        
        public static void RemoveFriend(string playerId)
        {
            PlaytoliaSocialPlatformFunc.RemoveFriend(playerId);
        }

        public static List<Player> SearchPlayers(string query) => PlaytoliaSocialPlatformFunc.SearchPlayers(query);
    }
}

