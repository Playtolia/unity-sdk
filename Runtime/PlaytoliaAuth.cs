using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Playtolia.Entity;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaAuth: VirtualStateful
    {
        private static PlaytoliaAuth _instance;
        
        public static PlaytoliaAuth Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaAuth();
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
        
        public static void PromptLogin(bool dismissable = true) {
            PlaytoliaAuthPlatformFunc.PromptLogin(dismissable);
        }
        
        public static void CancelLogin(bool dismissable = true) {
            PlaytoliaAuthPlatformFunc.CancelLogin(dismissable);
        }
        
        public static AuthState GetState()
        {
            return PlaytoliaAuthPlatformFunc.GetState();
        }

        public static void Logout(bool promptLogin = true)
        {
            PlaytoliaAuthPlatformFunc.Logout();
            if (promptLogin)
            {
                PromptLogin();
            }
            else
            {
                CancelLogin();
            }
        }

        public static bool IsLoggedIn() => PlaytoliaAuthPlatformFunc.IsLoggedIn();

        public static string GetAccessToken() => PlaytoliaAuthPlatformFunc.GetAccessToken();
    }
}