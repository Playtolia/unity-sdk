using System;
using System.Runtime.InteropServices;
using Playtolia.Entity;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaSession: VirtualStateful
    {
        private static PlaytoliaSession _instance;
        
        public static PlaytoliaSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaSession();
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
        
        public static User GetUser()
        {
            return PlaytoliaSessionPlatformFunc.GetState();
        }

        public static void Refresh()
        {
            PlaytoliaSessionPlatformFunc.Refresh();
        }

        public static void UpdateUsername(string username) => PlaytoliaSessionPlatformFunc.UpdateUsername(username);

        public static void UpdateDisplayName(string displayName) => PlaytoliaSessionPlatformFunc.UpdateDisplayName(displayName);

        public static void UpdatePassword(string oldPassword, string newPassword) => PlaytoliaSessionPlatformFunc.UpdatePassword(oldPassword, newPassword);
    }
}