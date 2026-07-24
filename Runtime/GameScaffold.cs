using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Playtolia.Entity.Scaffold;
using PlaytoliaSDK.Runtime.Common;
using PlaytoliaSDK.Runtime.Common.Core;

namespace PlaytoliaSDK.Runtime
{
    public class GameScaffold
    {
        public static void HideOverlay()
        { 
            PlaytoliaScaffoldPlatformFunc.HideOverlay();
        }
        
        public static void ShowOverlay()
        {
            PlaytoliaScaffoldPlatformFunc.ShowOverlay();
        }

        /// <summary>Move the floating button to a screen corner.</summary>
        public static void SetOverlayButtonAnchor(OverlayButtonAnchor anchor)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonAnchor(anchor);
        }

        /// <summary>Collapse or expand the floating button against its anchored side.</summary>
        public static void SetOverlayButtonMinimized(bool minimized)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonMinimized(minimized);
        }

        /// <summary>Enable or disable player-driven dragging.</summary>
        public static void SetOverlayButtonDraggingEnabled(bool enabled)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonDraggingEnabled(enabled);
        }

        /// <summary>Set the background using #RRGGBB or #AARRGGBB.</summary>
        public static void SetOverlayButtonBackgroundColor(string hexColor)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonBackgroundColor(hexColor);
        }

        /// <summary>Set active background opacity. Values are clamped to 0..1 natively.</summary>
        public static void SetOverlayButtonOpacity(float opacity)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonOpacity(opacity);
        }

        /// <summary>Set idle glass background opacity. Values are clamped to 0..1 natively.</summary>
        public static void SetOverlayButtonIdleOpacity(float opacity)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonIdleOpacity(opacity);
        }

        /// <summary>Set the inactivity delay before the glass appearance is applied.</summary>
        public static void SetOverlayButtonIdleDelay(int delayMilliseconds)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonIdleDelay(delayMilliseconds);
        }
        
        public static void Induce(InducedScaffoldEvent inducedEvent)
        {
            PlaytoliaScaffoldPlatformFunc.Induce(inducedEvent);
        }

        /// <summary>
        /// Opens the Playtolia overlay menu, exactly as tapping the floating Playtolia
        /// button would. Signed-out players are shown the login prompt instead.
        /// Shorthand for <see cref="Induce"/> with <see cref="InducedScaffoldEvent.UIShowMenu"/>.
        /// </summary>
        public static void LaunchMenu()
        {
            Induce(InducedScaffoldEvent.UIShowMenu);
        }
    }
}
