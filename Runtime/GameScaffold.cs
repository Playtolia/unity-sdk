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