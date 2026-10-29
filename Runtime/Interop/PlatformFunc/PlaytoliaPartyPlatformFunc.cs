using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Party;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaPartyPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Party_Refresh();
        [DllImport("__Internal")] private static extern IntPtr Party_SerializeState();
        [DllImport("__Internal")] private static extern IntPtr Party_GetCurrentParty();
        [DllImport("__Internal")] private static extern IntPtr Party_GetMembers();
        [DllImport("__Internal")] private static extern bool Party_IsLeader();
        [DllImport("__Internal")] private static extern bool Party_IsInParty();
        [DllImport("__Internal")] private static extern bool Party_IsMatchmaking();
        [DllImport("__Internal")] private static extern IntPtr Party_GetMatchmakingTicket();
        [DllImport("__Internal")] private static extern void Party_CreateParty(int maxSize, bool generateJoinCode);
        [DllImport("__Internal")] private static extern void Party_JoinByCode(string joinCode);
        [DllImport("__Internal")] private static extern void Party_LeaveParty();
        [DllImport("__Internal")] private static extern void Party_KickMember(string memberId);
        [DllImport("__Internal")] private static extern void Party_TransferLeadership(string newLeaderId);
        [DllImport("__Internal")] private static extern void Party_SetReady(bool ready);
        [DllImport("__Internal")] private static extern void Party_InvitePlayer(string playerId);
        [DllImport("__Internal")] private static extern void Party_AcceptInvite(string inviteId);
        [DllImport("__Internal")] private static extern void Party_StartMatchmaking(string queueName);
        [DllImport("__Internal")] private static extern void Party_CancelMatchmaking();
#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_Refresh();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Party_SerializeState();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Party_GetCurrentParty();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Party_GetMembers();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] [return: MarshalAs(UnmanagedType.I1)] private static extern bool Party_IsLeader();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] [return: MarshalAs(UnmanagedType.I1)] private static extern bool Party_IsInParty();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] [return: MarshalAs(UnmanagedType.I1)] private static extern bool Party_IsMatchmaking();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr Party_GetMatchmakingTicket();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_CreateParty(int maxSize, [MarshalAs(UnmanagedType.I1)] bool generateJoinCode);
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_JoinByCode(byte[] joinCode);
        private static void Party_JoinByCode(string joinCode) { Party_JoinByCode(NativeStrings.ToUtf8(joinCode)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_LeaveParty();
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_KickMember(byte[] memberId);
        private static void Party_KickMember(string memberId) { Party_KickMember(NativeStrings.ToUtf8(memberId)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_TransferLeadership(byte[] newLeaderId);
        private static void Party_TransferLeadership(string newLeaderId) { Party_TransferLeadership(NativeStrings.ToUtf8(newLeaderId)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_SetReady([MarshalAs(UnmanagedType.I1)] bool ready);
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_InvitePlayer(byte[] playerId);
        private static void Party_InvitePlayer(string playerId) { Party_InvitePlayer(NativeStrings.ToUtf8(playerId)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_AcceptInvite(byte[] inviteId);
        private static void Party_AcceptInvite(string inviteId) { Party_AcceptInvite(NativeStrings.ToUtf8(inviteId)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_StartMatchmaking(byte[] queueName);
        private static void Party_StartMatchmaking(string queueName) { Party_StartMatchmaking(NativeStrings.ToUtf8(queueName)); }
        [DllImport(NativeStrings.Library, CallingConvention = CallingConvention.Cdecl)] private static extern void Party_CancelMatchmaking();
#else
        private static void Party_Refresh() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static IntPtr Party_SerializeState() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static IntPtr Party_GetCurrentParty() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static IntPtr Party_GetMembers() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static bool Party_IsLeader() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static bool Party_IsInParty() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static bool Party_IsMatchmaking() { PlaytoliaInterop.UnsupportedPlatform(); return false; }
        private static IntPtr Party_GetMatchmakingTicket() { PlaytoliaInterop.UnsupportedPlatform(); return IntPtr.Zero; }
        private static void Party_CreateParty(int maxSize, bool generateJoinCode) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_JoinByCode(string joinCode) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_LeaveParty() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_KickMember(string memberId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_TransferLeadership(string newLeaderId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_SetReady(bool ready) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_InvitePlayer(string playerId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_AcceptInvite(string inviteId) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_StartMatchmaking(string queueName) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Party_CancelMatchmaking() { PlaytoliaInterop.UnsupportedPlatform(); }
#endif

        internal static void Refresh()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_Refresh();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "refresh");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static PartyState GetState()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr statePtr = Party_SerializeState();
                    if (statePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaPartyPlatformFunc: GetState returned null or empty value.");
                        return new PartyState();
                    }
                    
                    string stateString = Marshal.PtrToStringAnsi(statePtr);
                    Marshal.FreeHGlobal(statePtr);
                    
                    if (string.IsNullOrEmpty(stateString))
                    {
                        Debug.LogWarning("PlaytoliaPartyPlatformFunc: GetState returned null or empty value.");
                        return new PartyState();
                    }
                    return JsonConvert.DeserializeObject<PartyState>(stateString);
                    
                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winStatePtr = Party_SerializeState();
                    if (winStatePtr == IntPtr.Zero)
                    {
                        Debug.LogWarning("PlaytoliaPartyPlatformFunc: GetState returned null or empty value.");
                        return new PartyState();
                    }
                    
                    string winStateString = NativeStrings.TakeUtf8(winStatePtr);
                    
                    if (string.IsNullOrEmpty(winStateString))
                    {
                        Debug.LogWarning("PlaytoliaPartyPlatformFunc: GetState returned null or empty value.");
                        return new PartyState();
                    }
                    return JsonConvert.DeserializeObject<PartyState>(winStateString);
                }
                case RuntimePlatform.Android:
                    string androidStateString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "serializeState", true);
                    if (string.IsNullOrEmpty(androidStateString))
                    {
                        Debug.LogWarning("PlaytoliaPartyPlatformFunc: GetState returned null or empty value.");
                        return new PartyState();
                    }
                    return JsonConvert.DeserializeObject<PartyState>(androidStateString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new PartyState();
            }
        }

        internal static Party GetCurrentParty()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr partyPtr = Party_GetCurrentParty();
                    if (partyPtr == IntPtr.Zero)
                    {
                        return null;
                    }
                    
                    string partyString = Marshal.PtrToStringAnsi(partyPtr);
                    Marshal.FreeHGlobal(partyPtr);
                    
                    if (string.IsNullOrEmpty(partyString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<Party>(partyString);
                    
                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winPartyPtr = Party_GetCurrentParty();
                    if (winPartyPtr == IntPtr.Zero)
                    {
                        return null;
                    }
                    
                    string winPartyString = NativeStrings.TakeUtf8(winPartyPtr);
                    
                    if (string.IsNullOrEmpty(winPartyString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<Party>(winPartyString);
                }
                case RuntimePlatform.Android:
                    string androidPartyString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "getCurrentParty", true);
                    if (string.IsNullOrEmpty(androidPartyString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<Party>(androidPartyString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static List<PartyMember> GetMembers()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr membersPtr = Party_GetMembers();
                    if (membersPtr == IntPtr.Zero)
                    {
                        return new List<PartyMember>();
                    }
                    
                    string membersString = Marshal.PtrToStringAnsi(membersPtr);
                    Marshal.FreeHGlobal(membersPtr);
                    
                    if (string.IsNullOrEmpty(membersString))
                    {
                        return new List<PartyMember>();
                    }
                    return JsonConvert.DeserializeObject<List<PartyMember>>(membersString);
                    
                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winMembersPtr = Party_GetMembers();
                    if (winMembersPtr == IntPtr.Zero)
                    {
                        return new List<PartyMember>();
                    }
                    
                    string winMembersString = NativeStrings.TakeUtf8(winMembersPtr);
                    
                    if (string.IsNullOrEmpty(winMembersString))
                    {
                        return new List<PartyMember>();
                    }
                    return JsonConvert.DeserializeObject<List<PartyMember>>(winMembersString);
                }
                case RuntimePlatform.Android:
                    string androidMembersString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "getMembers", true);
                    if (string.IsNullOrEmpty(androidMembersString))
                    {
                        return new List<PartyMember>();
                    }
                    return JsonConvert.DeserializeObject<List<PartyMember>>(androidMembersString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return new List<PartyMember>();
            }
        }

        internal static bool IsLeader()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    return Party_IsLeader();
                case RuntimePlatform.Android:
                    string result = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "isLeader", true);
                    return !string.IsNullOrEmpty(result) && bool.Parse(result);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static bool IsInParty()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    return Party_IsInParty();
                case RuntimePlatform.Android:
                    string result = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "isInParty", true);
                    return !string.IsNullOrEmpty(result) && bool.Parse(result);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static bool IsMatchmaking()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    return Party_IsMatchmaking();
                case RuntimePlatform.Android:
                    string result = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "isMatchmaking", true);
                    return !string.IsNullOrEmpty(result) && bool.Parse(result);
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return false;
            }
        }

        internal static MatchmakingTicket GetMatchmakingTicket()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    IntPtr ticketPtr = Party_GetMatchmakingTicket();
                    if (ticketPtr == IntPtr.Zero)
                    {
                        return null;
                    }
                    
                    string ticketString = Marshal.PtrToStringAnsi(ticketPtr);
                    Marshal.FreeHGlobal(ticketPtr);
                    
                    if (string.IsNullOrEmpty(ticketString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<MatchmakingTicket>(ticketString);
                    
                case RuntimePlatform.WindowsPlayer:
                {
                    IntPtr winTicketPtr = Party_GetMatchmakingTicket();
                    if (winTicketPtr == IntPtr.Zero)
                    {
                        return null;
                    }
                    
                    string winTicketString = NativeStrings.TakeUtf8(winTicketPtr);
                    
                    if (string.IsNullOrEmpty(winTicketString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<MatchmakingTicket>(winTicketString);
                }
                case RuntimePlatform.Android:
                    string androidTicketString = PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "getMatchmakingTicket", true);
                    if (string.IsNullOrEmpty(androidTicketString))
                    {
                        return null;
                    }
                    return JsonConvert.DeserializeObject<MatchmakingTicket>(androidTicketString);
                    
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    return null;
            }
        }

        internal static void CreateParty(int maxSize, bool generateJoinCode)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_CreateParty(maxSize, generateJoinCode);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "createParty", false, maxSize, generateJoinCode);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void JoinByCode(string joinCode)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_JoinByCode(joinCode);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "joinByCode", false, joinCode);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void LeaveParty()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_LeaveParty();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "leaveParty");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void KickMember(string memberId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_KickMember(memberId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "kickMember", false, memberId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void TransferLeadership(string newLeaderId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_TransferLeadership(newLeaderId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "transferLeadership", false, newLeaderId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void SetReady(bool ready)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_SetReady(ready);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "setReady", false, ready);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void InvitePlayer(string playerId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_InvitePlayer(playerId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "invitePlayer", false, playerId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void AcceptInvite(string inviteId)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_AcceptInvite(inviteId);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "acceptInvite", false, inviteId);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void StartMatchmaking(string queueName)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_StartMatchmaking(queueName);
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "startMatchmaking", false, queueName);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void CancelMatchmaking()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WindowsPlayer:
                    Party_CancelMatchmaking();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Party, "cancelMatchmaking");
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}

