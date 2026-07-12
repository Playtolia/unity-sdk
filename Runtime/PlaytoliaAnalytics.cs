using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Playtolia.Entity.Analytics;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaAnalytics : VirtualStateful
    {
        private static PlaytoliaAnalytics _instance;

        public static PlaytoliaAnalytics Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaAnalytics();
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

        public static AnalyticsState GetState()
        {
            return PlaytoliaAnalyticsPlatformFunc.GetState();
        }

        /// <summary>
        /// Track a custom event with optional properties.
        /// </summary>
        /// <param name="eventName">Name of the event (e.g. "level_complete", "item_used").</param>
        /// <param name="properties">Optional key-value pairs attached to the event.</param>
        public static void Track(string eventName, Dictionary<string, object> properties = null)
        {
            string propertiesJson = properties != null && properties.Count > 0
                ? JsonConvert.SerializeObject(properties)
                : null;
            PlaytoliaAnalyticsPlatformFunc.Track(eventName, propertiesJson);
        }

        /// <summary>
        /// Returns the stable device ID, or null if analytics is not initialized.
        /// </summary>
        public static string GetDeviceId()
        {
            return PlaytoliaAnalyticsPlatformFunc.GetDeviceId();
        }

        /// <summary>
        /// Returns the current session ID, or null if analytics is not initialized.
        /// </summary>
        public static string GetSessionId()
        {
            return PlaytoliaAnalyticsPlatformFunc.GetSessionId();
        }

        /// <summary>
        /// Manually flush all pending analytics events.
        /// </summary>
        public static void Flush()
        {
            PlaytoliaAnalyticsPlatformFunc.Flush();
        }
    }
}
