using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Playtolia.Entity.Scaffold;
using Playtolia.Interop.Adapter;
using UnityEngine;

namespace PlaytoliaSDK.Runtime
{
    public class PlaytoliaScaffoldPlatformFunc
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport("__Internal")] private static extern void Scaffold_HideOverlay();
        [DllImport("__Internal")] private static extern void Scaffold_ShowOverlay();
        [DllImport("__Internal")] private static extern void Scaffold_Induce(string eventString);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonAnchor(string anchor);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonMinimized(bool minimized);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonDraggingEnabled(bool enabled);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonBackgroundColor(string color);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonOpacity(float opacity);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonIdleOpacity(float opacity);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonIdleDelay(long delayMilliseconds);
#else
        private static void Scaffold_HideOverlay() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_ShowOverlay() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_Induce(string eventString) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonAnchor(string anchor) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonMinimized(bool minimized) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonDraggingEnabled(bool enabled) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonBackgroundColor(string color) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonOpacity(float opacity) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonIdleOpacity(float opacity) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonIdleDelay(long delayMilliseconds) { PlaytoliaInterop.UnsupportedPlatform(); }
#endif
        
        internal static void HideOverlay() {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Scaffold_HideOverlay();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Scaffold, "hideOverlay", false);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
		}
        
        internal static void ShowOverlay() {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Scaffold_ShowOverlay();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Scaffold, "showOverlay", false);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
        
        internal static void Induce(InducedScaffoldEvent inducedEvent)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    Scaffold_Induce(inducedEvent.ToString());
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(InteropComponent.Scaffold, "induce", false, inducedEvent.ToString());
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }

        internal static void SetOverlayButtonAnchor(OverlayButtonAnchor anchor)
        {
            string nativeValue = System.Text.RegularExpressions.Regex.Replace(
                anchor.ToString(), "([a-z])([A-Z])", "$1_$2").ToUpperInvariant();
            Invoke(
                () => Scaffold_SetOverlayButtonAnchor(nativeValue),
                "setOverlayButtonAnchorByName",
                nativeValue);
        }

        internal static void SetOverlayButtonMinimized(bool minimized)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonMinimized(minimized),
                "setOverlayButtonMinimized",
                minimized);
        }

        internal static void SetOverlayButtonDraggingEnabled(bool enabled)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonDraggingEnabled(enabled),
                "setOverlayButtonDraggingEnabled",
                enabled);
        }

        internal static void SetOverlayButtonBackgroundColor(string color)
        {
            color = string.IsNullOrWhiteSpace(color) ? "#111416" : color;
            Invoke(
                () => Scaffold_SetOverlayButtonBackgroundColor(color),
                "setOverlayButtonBackgroundColor",
                color);
        }

        internal static void SetOverlayButtonOpacity(float opacity)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonOpacity(opacity),
                "setOverlayButtonOpacity",
                opacity);
        }

        internal static void SetOverlayButtonIdleOpacity(float opacity)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonIdleOpacity(opacity),
                "setOverlayButtonIdleOpacity",
                opacity);
        }

        internal static void SetOverlayButtonIdleDelay(int delayMilliseconds)
        {
            long nativeDelay = delayMilliseconds;
            Invoke(
                () => Scaffold_SetOverlayButtonIdleDelay(nativeDelay),
                "setOverlayButtonIdleDelayMillis",
                nativeDelay);
        }

        private static void Invoke(System.Action iosAction, string androidMethod, params object[] args)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    iosAction();
                    break;
                case RuntimePlatform.Android:
                    PlaytoliaAndroidInterop.ComponentCall(
                        InteropComponent.Scaffold, androidMethod, false, args);
                    break;
                default:
                    PlaytoliaInterop.UnsupportedPlatform();
                    break;
            }
        }
    }
}
