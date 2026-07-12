using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class InteropBridge
    {
        private static readonly Dictionary<string, CallbackEntry> Callbacks = new Dictionary<string, CallbackEntry>();

        public static string Register(Action<string> onSuccess, Action<string> onError = null)
        {
            var key = Guid.NewGuid().ToString();
            Callbacks[key] = new CallbackEntry(onSuccess, onError);
            return key;
        }

        public static void HandleMessage(string json)
        {
            BridgeMessage message;
            try
            {
                message = JsonConvert.DeserializeObject<BridgeMessage>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("InteropBridge: Failed to parse bridge message: " + e.Message);
                return;
            }

            if (message == null || string.IsNullOrEmpty(message.Key))
            {
                Debug.LogWarning("InteropBridge: Received bridge message with null or empty key.");
                return;
            }

            if (!Callbacks.TryGetValue(message.Key, out var entry))
            {
                Debug.LogWarning("InteropBridge: No callback registered for key: " + message.Key);
                return;
            }

            Callbacks.Remove(message.Key);

            if (message.Success)
            {
                entry.OnSuccess?.Invoke(message.Data);
            }
            else
            {
                entry.OnError?.Invoke(message.Error ?? "Unknown error");
            }
        }

        private class CallbackEntry
        {
            public readonly Action<string> OnSuccess;
            public readonly Action<string> OnError;

            public CallbackEntry(Action<string> onSuccess, Action<string> onError)
            {
                OnSuccess = onSuccess;
                OnError = onError;
            }
        }

        [Serializable]
        private class BridgeMessage
        {
            [JsonProperty("key")] public string Key;
            [JsonProperty("success")] public bool Success;
            [JsonProperty("error")] public string Error;
            [JsonProperty("data")] public string Data;
        }
    }
}
