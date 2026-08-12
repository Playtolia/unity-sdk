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

        /// <summary>Matches products by name, product code, or SKU.</summary>
        public static List<Product> SearchProducts(string query)
        {
            return PlaytoliaStorePlatformFunc.SearchItems(query);
        }

        public static void Refresh()
        {
            PlaytoliaStorePlatformFunc.Refresh();
        }

        public static List<Product> GetProductsByType(ProductType type)
        {
            return PlaytoliaStorePlatformFunc.GetItemsByType(type.ToString());
        }

        /// <summary>
        /// Looks a product up by the UUID Playtolia generated for it. Prefer
        /// <see cref="GetProductByCode"/> in game code: the code is the identifier you chose and
        /// can hardcode, the ID is not.
        /// </summary>
        public static Product GetProductById(string id)
        {
            return PlaytoliaStorePlatformFunc.GetItemById(id);
        }

        /// <summary>
        /// Looks a product up by the code you assigned it when creating it (for example
        /// "gold_500"). The code is fixed at creation and identical on every platform.
        /// </summary>
        public static Product GetProductByCode(string code)
        {
            return PlaytoliaStorePlatformFunc.GetItemByCode(code);
        }

        /// <summary>
        /// Looks a product up by its platform store SKU. A SKU only identifies the product on the
        /// store it is sold through, so the same product has a different SKU on iOS and Android.
        /// </summary>
        public static Product GetProductBySku(string sku)
        {
            return PlaytoliaStorePlatformFunc.GetItemBySku(sku);
        }

        [Obsolete("Renamed to SearchProducts.")]
        public static List<Product> SearchItems(string query) => SearchProducts(query);

        [Obsolete("Renamed to GetProductsByType.")]
        public static List<Product> GetItemsByType(ProductType type) => GetProductsByType(type);

        [Obsolete("Renamed to GetProductById. This takes the Playtolia-generated UUID, not your product code — use GetProductByCode for that.")]
        public static Product GetItemById(string id) => GetProductById(id);

        [Obsolete("Renamed to GetProductBySku.")]
        public static Product GetItemBySku(string sku) => GetProductBySku(sku);

        public static void BeginPurchaseFlow(string itemId, Action<PurchaseReceipt> onSuccess = null, Action<PurchaseError> onError = null)
        {
            PlaytoliaStorePlatformFunc.BeginPurchaseFlow(
                itemId,
                dataJson => DeliverReceipt(dataJson, onSuccess, onError),
                errorJson => DeliverError(errorJson, onError)
            );
        }

        private static void DeliverReceipt(string dataJson, Action<PurchaseReceipt> onSuccess, Action<PurchaseError> onError)
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
        }

        private static void DeliverError(string errorJson, Action<PurchaseError> onError)
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

        public static void BeginPurchaseFlow(Product product, Action<PurchaseReceipt> onSuccess = null, Action<PurchaseError> onError = null)
        {
            BeginPurchaseFlow(product.Id, onSuccess, onError);
        }

        /// <summary>
        /// Starts the purchase flow for the product with the given product code — the identifier
        /// you assigned at creation, not the Playtolia UUID.
        /// </summary>
        public static void BeginPurchaseFlowByCode(string code, Action<PurchaseReceipt> onSuccess = null, Action<PurchaseError> onError = null)
        {
            PlaytoliaStorePlatformFunc.BeginPurchaseFlowByCode(
                code,
                dataJson => DeliverReceipt(dataJson, onSuccess, onError),
                errorJson => DeliverError(errorJson, onError)
            );
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
        public static void BeginPurchaseFlow(Product product, Action onSuccess, Action<string> onError = null)
        {
            BeginPurchaseFlow(product.Id, onSuccess, onError);
        }

        public static List<Product> GetState()
        {
            return PlaytoliaStorePlatformFunc.GetState();
        }

        public static List<Product> GetProducts()
        {
            return PlaytoliaStorePlatformFunc.GetState();
        }

        [Obsolete("Renamed to GetProducts.")]
        public static List<Product> GetItems() => GetProducts();
    }
}
