using System;
using Newtonsoft.Json;
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

        /// <summary>
        /// Asks the backend to delete the signed-in player's account.
        ///
        /// Deletion is scheduled, not immediate: the account stays intact until
        /// <see cref="AccountDeletion.DeletionScheduledAt"/>, and logging in again before then
        /// cancels it. That grace period is deliberate, so this call does <b>not</b> log the
        /// player out — call <c>PlaytoliaAuth.Logout()</c> yourself and send them back to the
        /// sign-in screen if you want the deletion to go through.
        ///
        /// Asking twice fails; the first request stands.
        /// </summary>
        /// <param name="onSuccess">Called with the scheduled deletion once the backend accepts it.</param>
        /// <param name="onError">Called with the failure message if it does not.</param>
        public static void RequestAccountDeletion(Action<AccountDeletion> onSuccess = null, Action<string> onError = null)
        {
            PlaytoliaSessionPlatformFunc.RequestAccountDeletion(
                dataJson => DeliverAccountDeletion(dataJson, onSuccess, onError),
                errorJson => onError?.Invoke(errorJson)
            );
        }

        private static void DeliverAccountDeletion(string dataJson, Action<AccountDeletion> onSuccess, Action<string> onError)
        {
            if (onSuccess == null) return;
            try
            {
                onSuccess.Invoke(JsonConvert.DeserializeObject<AccountDeletion>(dataJson));
            }
            catch (Exception e)
            {
                onError?.Invoke("Failed to parse the account deletion response: " + e.Message);
            }
        }
    }
}