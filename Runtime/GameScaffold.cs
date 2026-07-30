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

        /// <summary>
        /// Set the background opacity used while the button is minimized against an edge.
        /// Values are clamped to 0..1 natively.
        /// </summary>
        public static void SetOverlayButtonMinimizedOpacity(float opacity)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonMinimizedOpacity(opacity);
        }

        /// <summary>
        /// Move the floating button to an arbitrary position. Both values are fractions of the
        /// button's free travel area: x runs from 0 at the start margin to 1 at the end margin,
        /// y from 0 at the top margin to 1 at the bottom margin. Values are clamped natively.
        /// </summary>
        public static void SetOverlayButtonPosition(float x, float y)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonPosition(x, y);
        }

        /// <summary>Choose how the button settles when the player releases a drag.</summary>
        public static void SetOverlayButtonSnapMode(OverlayButtonSnapMode snapMode)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonSnapMode(snapMode);
        }

        /// <summary>
        /// Enable or disable auto-hide. When enabled the button minimizes itself against its
        /// current side after a period without interaction, and comes back when the player taps
        /// or swipes the remaining notch.
        /// </summary>
        public static void SetOverlayButtonAutoHideEnabled(bool enabled)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonAutoHideEnabled(enabled);
        }

        /// <summary>Set the inactivity delay before auto-hide minimizes the button.</summary>
        public static void SetOverlayButtonAutoHideDelay(int delayMilliseconds)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayButtonAutoHideDelay(delayMilliseconds);
        }
        
        /// <summary>
        /// Choose whether the overlay menu and its screens open as a bottom sheet or as a side
        /// panel. Changing this while the menu is open takes effect the next time it opens.
        /// </summary>
        public static void SetOverlayMenuPresentation(OverlayMenuPresentation presentation)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayMenuPresentation(presentation);
        }

        /// <summary>
        /// Set the side panel width as a fraction of the screen, used with
        /// <see cref="OverlayMenuPresentation.LeftPanel"/>. Clamped to 0.2..1 natively.
        /// </summary>
        public static void SetOverlayMenuPanelWidthFraction(float fraction)
        {
            PlaytoliaScaffoldPlatformFunc.SetOverlayMenuPanelWidthFraction(fraction);
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
