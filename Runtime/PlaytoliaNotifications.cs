using System;
using Playtolia.Entity.Push;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaNotifications : VirtualStateful
    {
        private static PlaytoliaNotifications _instance;

        public static PlaytoliaNotifications Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaNotifications();
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
            PlaytoliaNotificationsPlatformFunc.Refresh();
        }

        public static PushState GetState()
        {
            return PlaytoliaNotificationsPlatformFunc.GetState();
        }

        public static void RegisterPushToken(string token)
        {
            PlaytoliaNotificationsPlatformFunc.RegisterPushToken(token);
        }

        public static void RequestPushPermission()
        {
            PlaytoliaNotificationsPlatformFunc.RequestPushPermission();
        }

        public static void OpenNotificationSettings()
        {
            PlaytoliaNotificationsPlatformFunc.OpenNotificationSettings();
        }

        public static string GetPermission()
        {
            return PlaytoliaNotificationsPlatformFunc.GetPermission();
        }

        public static void SendTestPush(string title, string body, string data = null)
        {
            PlaytoliaNotificationsPlatformFunc.SendTestPush(title, body, data);
        }
    }
}
