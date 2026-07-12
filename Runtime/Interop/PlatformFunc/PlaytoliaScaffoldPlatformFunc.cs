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
#else
        private static void Scaffold_HideOverlay() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_ShowOverlay() { PlaytoliaInterop.UnsupportedPlatform(); }
        private static void Scaffold_Induce(string eventString) { PlaytoliaInterop.UnsupportedPlatform(); }
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
    }
}