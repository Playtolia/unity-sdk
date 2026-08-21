namespace Playtolia.Editor
{
    public class PlaytoliaConfigurationHelper
    {
        private string tmp = "";
        
        public string SerializeSettings()
        {
            tmp = "{";
            SetValue("gameId", PlaytoliaEditorSettings.GetGameId());
            SetValue("headless", PlaytoliaEditorSettings.IsHeadlessEnabled());
            SetValue("overlayButtonDraggingEnabled",
                PlaytoliaEditorSettings.IsOverlayButtonDraggingEnabled());
            SetValue("overlayButtonSnapMode", PlaytoliaEditorSettings.GetOverlayButtonSnapMode());
            SetValue("overlayButtonMinimizedOpacity",
                PlaytoliaEditorSettings.GetOverlayButtonMinimizedOpacity());
            SetValue("overlayButtonBackgroundColor",
                PlaytoliaEditorSettings.GetOverlayButtonBackgroundColor());
            var expandedIcon = PlaytoliaEditorSettings.GetOverlayButtonIconDataUri();
            if (!string.IsNullOrEmpty(expandedIcon)) SetValue("overlayButtonIcon", expandedIcon);
            var collapsedIcon = PlaytoliaEditorSettings.GetOverlayButtonMinimizedIconDataUri();
            if (!string.IsNullOrEmpty(collapsedIcon)) SetValue("overlayButtonMinimizedIcon", collapsedIcon);
            var expandedArtwork = PlaytoliaEditorSettings.GetOverlayButtonExpandedArtworkDataUri();
            if (!string.IsNullOrEmpty(expandedArtwork)) SetValue("overlayButtonExpandedArtwork", expandedArtwork);
            var collapsedArtwork = PlaytoliaEditorSettings.GetOverlayButtonMinimizedArtworkDataUri();
            if (!string.IsNullOrEmpty(collapsedArtwork)) SetValue("overlayButtonMinimizedArtwork", collapsedArtwork);
            SetValue("overlayButtonArtworkFollowsEdge", PlaytoliaEditorSettings.GetOverlayButtonArtworkFollowsEdge());
            SetValue("overlayButtonShowIcon", PlaytoliaEditorSettings.GetOverlayButtonShowIcon());
            SetValue("overlayButtonShowMinimizedIcon", PlaytoliaEditorSettings.GetOverlayButtonShowMinimizedIcon());
            SetValue("overlayButtonSizeDp", PlaytoliaEditorSettings.GetOverlayButtonSizeDp());
            SetValue("overlayButtonExpandedLengthDp", PlaytoliaEditorSettings.GetOverlayButtonExpandedLengthDp());
            SetValue("overlayButtonExpandedThicknessDp", PlaytoliaEditorSettings.GetOverlayButtonExpandedThicknessDp());
            SetValue("overlayButtonCornerRadiusDp", PlaytoliaEditorSettings.GetOverlayButtonCornerRadiusDp());
            SetValue("overlayButtonMinimizedLengthDp", PlaytoliaEditorSettings.GetOverlayButtonMinimizedLengthDp());
            SetValue("overlayButtonMinimizedThicknessDp", PlaytoliaEditorSettings.GetOverlayButtonMinimizedThicknessDp());
            SetValue("overlayButtonMinimizedCornerRadiusDp", PlaytoliaEditorSettings.GetOverlayButtonMinimizedCornerRadiusDp());
            SetValue("overlayButtonRespectSafeArea", PlaytoliaEditorSettings.GetOverlayButtonRespectSafeArea());
            SetValue("overlayButtonSafeAreaPaddingDp", PlaytoliaEditorSettings.GetOverlayButtonSafeAreaPaddingDp());
            SetValue("overlayButtonAutoHideEnabled",
                PlaytoliaEditorSettings.IsOverlayButtonAutoHideEnabled());
            if (PlaytoliaEditorSettings.IsOverlayButtonAutoHideEnabled())
            {
                SetValue("overlayButtonAutoHideDelayMillis",
                    PlaytoliaEditorSettings.GetOverlayButtonAutoHideDelayMilliseconds());
            }
            
            // Custom Origin
            if (PlaytoliaEditorSettings.IsCustomOriginEnabled())
            {
                var customOrigin = PlaytoliaEditorSettings.GetCustomOrigin();
                if (!string.IsNullOrEmpty(customOrigin))
                {
                    SetValue("overrideBaseUrl", customOrigin);
                }
            }
            
            SetValue("enablePersistence", true);
            SetValue("enableAuthentication", PlaytoliaEditorSettings.IsAuthEnabled());
            SetValue("authenticationRequired", PlaytoliaEditorSettings.IsAuthRequired());
            SetValue("googleAuthentication", PlaytoliaEditorSettings.IsAuthProviderGoogleEnabled());
            SetValue("facebookAuthentication", PlaytoliaEditorSettings.IsAuthProviderFacebookEnabled());
            SetValue("appleAuthentication", PlaytoliaEditorSettings.IsAuthProviderAppleEnabled());
            SetValue("discordAuthentication", PlaytoliaEditorSettings.IsAuthProviderDiscordEnabled());
            SetValue("guestAuthentication", PlaytoliaEditorSettings.IsAuthProviderPlayAsGuestEnabled());
            SetValue("enableTicketing", PlaytoliaEditorSettings.IsTicketingEnabled());
            SetValue("enableBilling", PlaytoliaEditorSettings.IsBillingEnabled());
            SetValue("enablePush", PlaytoliaEditorSettings.IsPushEnabled());
            SetValue("autoPushPermissionCheck", PlaytoliaEditorSettings.IsAutoPushPermissionCheckEnabled());
            SetValue("enableSocial", PlaytoliaEditorSettings.IsSocialEnabled());
            SetValue("enableCrashCapture", PlaytoliaEditorSettings.IsCrashCaptureEnabled());
            SetValue("enableAnalytics", PlaytoliaEditorSettings.IsAnalyticsEnabled());
            if (PlaytoliaEditorSettings.IsAnalyticsEnabled())
            {
                SetValue("grainTenantAlias", PlaytoliaEditorSettings.GetGameId());
                SetValue("autoTrackEvents", PlaytoliaEditorSettings.IsAutoTrackEventsEnabled());
            }

            // Attribution
            var attributionProvider = PlaytoliaEditorSettings.GetAttributionProvider();
            SetValue("attributionProvider", attributionProvider.ToString().ToLower());
            if (attributionProvider != AttributionProvider.None)
            {
                SetValue("attributionStoreAccountAttribute",
                    PlaytoliaEditorSettings.IsAttributionStoreAccountAttributeEnabled());
            }
            if (attributionProvider == AttributionProvider.Airbridge)
            {
                SetValue("airBridgeAppName", PlaytoliaEditorSettings.GetAirBridgeAppName());
                SetValue("airBridgeAppToken", PlaytoliaEditorSettings.GetAirBridgeAppToken());
                SetValue("airBridgeEnvironment",
                    PlaytoliaEditorSettings.IsAirBridgeSandbox() ? "sandbox" : "production");
            }
            SetValue("overlayMenuPresentation", PlaytoliaEditorSettings.GetOverlayMenuPresentation());
            SetValue("overlayMenuPanelWidthFraction",
                PlaytoliaEditorSettings.GetOverlayMenuPanelWidthFraction());
            SetValue("showProfileSection", PlaytoliaEditorSettings.IsProfileSectionVisible());
            SetValue("showSettingsTile", PlaytoliaEditorSettings.IsSettingsTileVisible());
            SetValue("showSupportTile", PlaytoliaEditorSettings.IsSupportTileVisible());
            SetValue("showFriendsTile", PlaytoliaEditorSettings.IsFriendsTileVisible());
            SetValue("showChatTile", PlaytoliaEditorSettings.IsChatTileVisible());
            SetValue("showStoreTile", PlaytoliaEditorSettings.IsStoreTileVisible());
            SetValue("showRewardsTile", PlaytoliaEditorSettings.IsRewardsTileVisible());
            SetValue("showTasksTile", PlaytoliaEditorSettings.IsTasksTileVisible());
            SetValue("showLeaderboardTile", PlaytoliaEditorSettings.IsLeaderboardTileVisible());

            // Legal - only emit when set; omitted links render as non-clickable text
            var termsUrl = PlaytoliaEditorSettings.GetTermsOfServiceUrl();
            if (!string.IsNullOrEmpty(termsUrl))
            {
                SetValue("termsOfServiceUrl", termsUrl);
            }
            var privacyUrl = PlaytoliaEditorSettings.GetPrivacyPolicyUrl();
            if (!string.IsNullOrEmpty(privacyUrl))
            {
                SetValue("privacyPolicyUrl", privacyUrl);
            }

            SetValue("debug", PlaytoliaEditorSettings.IsDebugModeEnabled());
            SetValue("loggingLevel", PlaytoliaEditorSettings.GetLoggingLevel().ToString());
            tmp += "}";
            return tmp;
        }

        void SetValueRaw(string key, string value)
        {
            var pair = "\"" + key + "\": " + value;
            if (tmp.Length > 1) tmp = tmp + ", ";
            tmp = tmp + pair;
        }
        
        void SetValue(string key, string value)
        {
            SetValueRaw(key, "\"" + value + "\"");
        }
        
        void SetValue(string key, bool value)
        {
            SetValueRaw(key, value ? "true" : "false");
        }

        void SetValue(string key, int value)
        {
            SetValueRaw(key, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        // Invariant culture, so a comma decimal separator never produces invalid JSON.
        void SetValue(string key, float value)
        {
            SetValueRaw(key, value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
