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
    }
}