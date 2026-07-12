using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Social;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSocialPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Social_Refresh();
        [DllImport("__Internal")] private static extern IntPtr Social_SerializeState();
        [DllImport("__Internal")] private static extern IntPtr Social_GetFriends();
        [DllImport("__Internal")] private static extern IntPtr Social_GetIncomingFriendRequests();
        [DllImport("__Internal")] private static extern IntPtr Social_GetOutgoingFriendRequests();
        [DllImport("__Internal")] private static extern void Social_SendFriendRequest(string playerId);
        [DllImport("__Internal")] private static extern void Social_AcceptFriendRequest(string requestId);
        [DllImport("__Internal")] private static extern void Social_RejectFriendRequest(string requestId);
        [DllImport("__Internal")] private static extern void Social_CancelFriendRequest(string requestId);
        [DllImport("__Internal")] private static extern void Social_RemoveFriend(string friendshipId);
        [DllImport("__Internal")] private static extern IntPtr Social_SearchPlayers(string query);
#else
        private static void Social_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Social_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static IntPtr Social_GetFriends() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static IntPtr Social_GetIncomingFriendRequests() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static IntPtr Social_GetOutgoingFriendRequests() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static void Social_SendFriendRequest(string playerId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Social_AcceptFriendRequest(string requestId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Social_RejectFriendRequest(string requestId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Social_CancelFriendRequest(string requestId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Social_RemoveFriend(string friendshipId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Social_SearchPlayers(string query) { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
#endif

        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static SocialState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr statePtr = Social_SerializeState();
                    if (statePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaSocialPlatformFunc: GetState returned null or empty value.");
                        return new SocialState();
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaSocialPlatformFunc: GetState returned null or empty value.");
                        return new SocialState();
                    }
                    return JsonConvert.DeserializeObject<SocialState>(stateString);
                    
                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaSocialPlatformFunc: GetState returned null or empty value.");
                        return new SocialState();
                    }
                    return JsonConvert.DeserializeObject<SocialState>(androidStateString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new SocialState();
            }
        }

        internal static List<Friend> GetFriends()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr friendsPtr = Social_GetFriends();
                    if (friendsPtr == IntPtr.Zero)
                    {
                        return new List<Friend>();
                    }
                    
                    string friendsString = Marshal.PtrToStringAnsi(friendsPtr);
                    Marshal.FreeHGlobal(friendsPtr);
                    
                    if (string.IsNullOrEmpty(friendsString))
                    {
                        return new List<Friend>();
                    }
                    return JsonConvert.DeserializeObject<List<Friend>>(friendsString);
                    
                case RuntimePlatform.Android:
                    string androidFriendsString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "getFriends", true);
                    if (string.IsNullOrEmpty(androidFriendsString))
                    {
                        return new List<Friend>();
                    }
                    return JsonConvert.DeserializeObject<List<Friend>>(androidFriendsString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new List<Friend>();
            }
        }

        internal static List<FriendRequest> GetIncomingFriendRequests()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr requestsPtr = Social_GetIncomingFriendRequests();
                    if (requestsPtr == IntPtr.Zero)
                    {
                        return new List<FriendRequest>();
                    }
                    
                    string requestsString = Marshal.PtrToStringAnsi(requestsPtr);
                    Marshal.FreeHGlobal(requestsPtr);
                    
                    if (string.IsNullOrEmpty(requestsString))
                    {
                        return new List<FriendRequest>();
                    }
                    return JsonConvert.DeserializeObject<List<FriendRequest>>(requestsString);
                    
                case RuntimePlatform.Android:
                    string androidRequestsString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "getIncomingFriendRequests", true);
                    if (string.IsNullOrEmpty(androidRequestsString))
                    {
                        return new List<FriendRequest>();
                    }
                    return JsonConvert.DeserializeObject<List<FriendRequest>>(androidRequestsString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new List<FriendRequest>();
            }
        }

        internal static List<FriendRequest> GetOutgoingFriendRequests()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr requestsPtr = Social_GetOutgoingFriendRequests();
                    if (requestsPtr == IntPtr.Zero)
                    {
                        return new List<FriendRequest>();
                    }
                    
                    string requestsString = Marshal.PtrToStringAnsi(requestsPtr);
                    Marshal.FreeHGlobal(requestsPtr);
                    
                    if (string.IsNullOrEmpty(requestsString))
                    {
                        return new List<FriendRequest>();
                    }
                    return JsonConvert.DeserializeObject<List<FriendRequest>>(requestsString);
                    
                case RuntimePlatform.Android:
                    string androidRequestsString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "getOutgoingFriendRequests", true);
                    if (string.IsNullOrEmpty(androidRequestsString))
                    {
                        return new List<FriendRequest>();
                    }
                    return JsonConvert.DeserializeObject<List<FriendRequest>>(androidRequestsString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new List<FriendRequest>();
            }
        }

        internal static void SendFriendRequest(string playerId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_SendFriendRequest(playerId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "sendFriendRequest", false, playerId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void AcceptFriendRequest(string requestId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_AcceptFriendRequest(requestId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "acceptFriendRequest", false, requestId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void RejectFriendRequest(string requestId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_RejectFriendRequest(requestId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "rejectFriendRequest", false, requestId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void CancelFriendRequest(string requestId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_CancelFriendRequest(requestId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "cancelFriendRequest", false, requestId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void RemoveFriend(string friendshipId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Social_RemoveFriend(friendshipId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "removeFriend", false, friendshipId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static List<Player> SearchPlayers(string query)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr resultPtr = Social_SearchPlayers(query);
                    if (resultPtr == IntPtr.Zero) return new List<Player>();
                    string resultString = Marshal.PtrToStringAnsi(resultPtr);
                    Marshal.FreeHGlobal(resultPtr);
                    if (string.IsNullOrEmpty(resultString)) return new List<Player>();
                    return JsonConvert.DeserializeObject<List<Player>>(resultString) ?? new List<Player>();
                case RuntimePlatform.Android:
                    string androidResult = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Social, "searchPlayers", true, query);
                    if (string.IsNullOrEmpty(androidResult)) return new List<Player>();
                    return JsonConvert.DeserializeObject<List<Player>>(androidResult) ?? new List<Player>();
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new List<Player>();
            }
        }
    }
}

