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
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonMinimizedOpacity(float opacity);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonPosition(float x, float y);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonSnapMode(string snapMode);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonAutoHideEnabled(bool enabled);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayButtonAutoHideDelay(long delayMilliseconds);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayMenuPresentation(string presentation);
        [DllImport("__Internal")] private static extern void Scaffold_SetOverlayMenuPanelWidthFraction(float fraction);
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
        private static void Scaffold_SetOverlayButtonMinimizedOpacity(float opacity) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonPosition(float x, float y) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonSnapMode(string snapMode) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonAutoHideEnabled(bool enabled) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayButtonAutoHideDelay(long delayMilliseconds) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayMenuPresentation(string presentation) { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_SetOverlayMenuPanelWidthFraction(float fraction) { PlaytoliaInterop.UnsupportedPlatform(); }
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

        internal static void SetOverlayButtonMinimizedOpacity(float opacity)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonMinimizedOpacity(opacity),
                "setOverlayButtonMinimizedOpacity",
                opacity);
        }

        internal static void SetOverlayButtonPosition(float x, float y)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonPosition(x, y),
                "setOverlayButtonPosition",
                x, y);
        }

        internal static void SetOverlayButtonSnapMode(OverlayButtonSnapMode snapMode)
        {
            string nativeValue = snapMode.ToString().ToUpperInvariant();
            Invoke(
                () => Scaffold_SetOverlayButtonSnapMode(nativeValue),
                "setOverlayButtonSnapModeByName",
                nativeValue);
        }

        internal static void SetOverlayButtonAutoHideEnabled(bool enabled)
        {
            Invoke(
                () => Scaffold_SetOverlayButtonAutoHideEnabled(enabled),
                "setOverlayButtonAutoHideEnabled",
                enabled);
        }

        internal static void SetOverlayButtonAutoHideDelay(int delayMilliseconds)
        {
            long nativeDelay = delayMilliseconds;
            Invoke(
                () => Scaffold_SetOverlayButtonAutoHideDelay(nativeDelay),
                "setOverlayButtonAutoHideDelayMillis",
                nativeDelay);
        }

        internal static void SetOverlayMenuPresentation(OverlayMenuPresentation presentation)
        {
            string nativeValue = System.Text.RegularExpressions.Regex.Replace(
                presentation.ToString(), "([a-z])([A-Z])", "$1_$2").ToUpperInvariant();
            Invoke(
                () => Scaffold_SetOverlayMenuPresentation(nativeValue),
                "setOverlayMenuPresentationByName",
                nativeValue);
        }

        internal static void SetOverlayMenuPanelWidthFraction(float fraction)
        {
            Invoke(
                () => Scaffold_SetOverlayMenuPanelWidthFraction(fraction),
                "setOverlayMenuPanelWidthFraction",
                fraction);
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
