using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Playtolia.Entity;
using Playtolia.Entity.Billing;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaStore: VirtualStateful
    {
        private static PlaytoliaStore _instance;

        public static PlaytoliaStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaytoliaStore();
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

        public static List<StoreItem> SearchItems(string query)
        {
            return PlaytoliaStorePlatformFunc.SearchItems(query);
        }

        public static void Refresh()
        {
            PlaytoliaStorePlatformFunc.Refresh();
        }

        public static List<StoreItem> GetItemsByType(StoreItemType type)
        {
            return PlaytoliaStorePlatformFunc.GetItemsByType(type.ToString());
        }

        public static StoreItem GetItemById(string id)
        {
            return PlaytoliaStorePlatformFunc.GetItemById(id);
        }

        public static StoreItem GetItemBySku(string sku)
        {
            return PlaytoliaStorePlatformFunc.GetItemBySku(sku);
        }

        public static void BeginPurchaseFlow(string itemId, Action<PurchaseReceipt> onSuccess = null, Action<PurchaseError> onError = null)
        {
            PlaytoliaStorePlatformFunc.BeginPurchaseFlow(
                itemId,
                dataJson =>
                {
                    if (onSuccess == null) return;
                    try
                    {
                        var receipt = JsonConvert.DeserializeObject<PurchaseReceipt>(dataJson);
                        onSuccess.Invoke(receipt);
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError("PlaytoliaStore: Failed to deserialize PurchaseReceipt: " + e.Message);
                        onError?.Invoke(new PurchaseError { Code = "UNKNOWN", Message = "Failed to deserialize purchase receipt: " + e.Message });
                    }
                },
                errorJson =>
                {
                    if (onError == null) return;
                    try
                    {
                        var error = JsonConvert.DeserializeObject<PurchaseError>(errorJson);
                        onError.Invoke(error);
                    }
                    catch
                    {
                        onError.Invoke(new PurchaseError { Code = "UNKNOWN", Message = errorJson });
                    }
                }
            );
        }

        public static void BeginPurchaseFlow(StoreItem item, Action<PurchaseReceipt> onSuccess = null, Action<PurchaseError> onError = null)
        {
            BeginPurchaseFlow(item.Id, onSuccess, onError);
        }

        [Obsolete("Use the overload with Action<PurchaseReceipt> and Action<PurchaseError> for typed callback data.")]
        public static void BeginPurchaseFlow(string itemId, Action onSuccess, Action<string> onError = null)
        {
            BeginPurchaseFlow(
                itemId,
                receipt => onSuccess?.Invoke(),
                error => onError?.Invoke(error?.Message ?? "Unknown error")
            );
        }

        [Obsolete("Use the overload with Action<PurchaseReceipt> and Action<PurchaseError> for typed callback data.")]
        public static void BeginPurchaseFlow(StoreItem item, Action onSuccess, Action<string> onError = null)
        {
            BeginPurchaseFlow(item.Id, onSuccess, onError);
        }

        public static List<StoreItem> GetState()
        {
            return PlaytoliaStorePlatformFunc.GetState();
        }

        public static List<StoreItem> GetItems()
        {
            return PlaytoliaStorePlatformFunc.GetState();
        }
    }
}
