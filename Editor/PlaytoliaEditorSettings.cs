using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

enum LoggingLevel
{
    None,
    Error,
    Warning,
    Info,
    Debug
}

enum AttributionProvider
{
    None,
    System,
    Airbridge
}

// Create a new type of Settings Asset.
class PlaytoliaEditorSettings : ScriptableObject
{
    public const string k_PlaytoliaEditorSettingsPath = "Assets/Editor/PlaytoliaEditorSettings.asset";

    // Firebase-backed push is optional. This scripting define gates the Firebase code in
    // PlaytoliaGameObject so the package compiles without the Firebase Unity SDK. It is kept
    // in sync with the "Enable Push Notifications" toggle below, so enabling push turns the
    // Firebase integration on and disabling it compiles Firebase back out.
    private const string k_FirebaseDefine = "PLAYTOLIA_FIREBASE";

    // Cover the build targets a Playtolia game actually ships on, plus Standalone so the
    // define is also active when compiling for the Editor.
    private static readonly NamedBuildTarget[] k_FirebaseDefineTargets =
    {
        NamedBuildTarget.Android,
        NamedBuildTarget.iOS,
        NamedBuildTarget.Standalone,
    };

    // Add or remove PLAYTOLIA_FIREBASE across the relevant build targets to match the Push
    // setting. Only writes (which triggers a recompile) on an actual change.
    internal static void SyncFirebaseDefine(bool pushEnabled)
    {
        foreach (var target in k_FirebaseDefineTargets)
        {
            var defines = PlayerSettings.GetScriptingDefineSymbols(target)
                .Split(';')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
            bool has = defines.Contains(k_FirebaseDefine);

            if (pushEnabled && !has)
            {
                defines.Add(k_FirebaseDefine);
                PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
            }
            else if (!pushEnabled && has)
            {
                defines.RemoveAll(s => s == k_FirebaseDefine);
                PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
            }
        }
    }

    // Reconcile the define with the saved setting whenever the editor loads, so pulling a
    // project (or hand-editing the asset) keeps the define correct without re-toggling.
    // Loads the asset without creating it, so merely importing the package is a no-op.
    [InitializeOnLoadMethod]
    private static void ReconcileFirebaseDefineOnLoad()
    {
        EditorApplication.delayCall += () =>
        {
            var existing = AssetDatabase.LoadAssetAtPath<PlaytoliaEditorSettings>(k_PlaytoliaEditorSettingsPath);
            SyncFirebaseDefine(existing != null && existing.m_PushEnabled);
        };
    }

    [SerializeField] private string m_GameId;
    [SerializeField] private bool m_Headless;
    [SerializeField] private bool m_OverlayButtonDraggingEnabled = true;
    [SerializeField] private bool m_EnableCustomOrigin;
    [SerializeField] private string m_CustomOrigin;
    [SerializeField] private bool m_AuthEnabled;
    [SerializeField] private bool m_AuthRequired;
    [SerializeField] private bool m_AuthProviderGoogleEnabled;
    [SerializeField] private bool m_AuthProviderFacebookEnabled;
    [SerializeField] private bool m_AuthProviderAppleEnabled;
    [SerializeField] private bool m_AuthProviderDiscordEnabled;
    [SerializeField] private bool m_AuthProviderPlayAsGuestEnabled;
    [SerializeField] private bool m_TicketingEnabled;
    [SerializeField] private bool m_BillingEnabled;
    [SerializeField] private bool m_PushEnabled;
    [SerializeField] private bool m_AutoPushPermissionCheck;
    [SerializeField] private bool m_SocialEnabled;
    [SerializeField] private bool m_CrashCaptureEnabled;
    [SerializeField] private bool m_AnalyticsEnabled;
    [SerializeField] private bool m_AutoTrackEvents;

    // Attribution
    [SerializeField] private AttributionProvider m_AttributionProvider;
    [SerializeField] private bool m_AttributionStoreAccountAttribute;
    [SerializeField] private string m_AirBridgeAppName;
    [SerializeField] private string m_AirBridgeAppToken;
    [SerializeField] private bool m_AirBridgeSandbox;

    [SerializeField] private LoggingLevel m_LoggingLevel;
    [SerializeField] private bool m_AddEntitlements;
    [SerializeField] private bool m_DebugModeEnabled;

    // Legal
    [SerializeField] private string m_TermsOfServiceUrl;
    [SerializeField] private string m_PrivacyPolicyUrl;
    
    // Overlay Menu Visibility
    [SerializeField] private bool m_ShowProfileSection;
    [SerializeField] private bool m_ShowSettingsTile;
    [SerializeField] private bool m_ShowSupportTile;
    [SerializeField] private bool m_ShowFriendsTile;
    [SerializeField] private bool m_ShowChatTile;
    [SerializeField] private bool m_ShowStoreTile;
    [SerializeField] private bool m_ShowRewardsTile;
    [SerializeField] private bool m_ShowTasksTile;
    [SerializeField] private bool m_ShowLeaderboardTile;

    internal static PlaytoliaEditorSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<PlaytoliaEditorSettings>(k_PlaytoliaEditorSettingsPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<PlaytoliaEditorSettings>();
            settings.m_GameId = "";
            settings.m_Headless = false;
            settings.m_OverlayButtonDraggingEnabled = true;
            settings.m_EnableCustomOrigin = false;
            settings.m_CustomOrigin = "";
            settings.m_AuthEnabled = true;
            settings.m_AuthRequired = true;
            settings.m_AuthProviderGoogleEnabled = true;
            settings.m_AuthProviderFacebookEnabled = true;
            settings.m_AuthProviderAppleEnabled = true;
            settings.m_AuthProviderDiscordEnabled = true;
            settings.m_AuthProviderPlayAsGuestEnabled = true;
            settings.m_TicketingEnabled = true;
            settings.m_BillingEnabled = true;
            settings.m_PushEnabled = false;
            settings.m_AutoPushPermissionCheck = false;
            settings.m_SocialEnabled = true;
            settings.m_CrashCaptureEnabled = true;
            settings.m_AnalyticsEnabled = true;
            settings.m_AutoTrackEvents = true;
            settings.m_AttributionProvider = AttributionProvider.None;
            settings.m_AttributionStoreAccountAttribute = true;
            settings.m_AirBridgeAppName = "";
            settings.m_AirBridgeAppToken = "";
            settings.m_AirBridgeSandbox = false;
            settings.m_LoggingLevel = LoggingLevel.Info;
            settings.m_AddEntitlements = true;
            settings.m_DebugModeEnabled = true;

            // Legal - no default links; omitted links render as plain, non-clickable text
            settings.m_TermsOfServiceUrl = "";
            settings.m_PrivacyPolicyUrl = "";
            
            // Overlay Menu Visibility - all enabled by default
            settings.m_ShowProfileSection = true;
            settings.m_ShowSettingsTile = true;
            settings.m_ShowSupportTile = true;
            settings.m_ShowFriendsTile = true;
            settings.m_ShowChatTile = true;
            settings.m_ShowStoreTile = true;
            settings.m_ShowRewardsTile = true;
            settings.m_ShowTasksTile = true;
            settings.m_ShowLeaderboardTile = true;
            
            AssetDatabase.CreateAsset(settings, k_PlaytoliaEditorSettingsPath);
            AssetDatabase.SaveAssets();
        }

        return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }

    public static string GetGameId()
    {
        var settings = GetOrCreateSettings();
        return settings.m_GameId;
    }

    public static bool IsHeadlessEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_Headless;
    }

    public static bool IsOverlayButtonDraggingEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonDraggingEnabled;
    }

    public static bool IsCustomOriginEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_EnableCustomOrigin;
    }

    public static string GetCustomOrigin()
    {
        var settings = GetOrCreateSettings();
        return settings.m_CustomOrigin;
    }

    public static bool IsAuthEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthEnabled;
    }

    public static bool IsAuthRequired()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthRequired;
    }

    public static bool IsAuthProviderGoogleEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthProviderGoogleEnabled;
    }

    public static bool IsAuthProviderFacebookEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthProviderFacebookEnabled;
    }

    public static bool IsAuthProviderAppleEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthProviderAppleEnabled;
    }

    public static bool IsAuthProviderDiscordEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthProviderDiscordEnabled;
    }

    public static bool IsAuthProviderPlayAsGuestEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AuthProviderPlayAsGuestEnabled;
    }

    public static bool IsTicketingEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_TicketingEnabled;
    }

    public static bool IsBillingEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_BillingEnabled;
    }

    public static bool IsPushEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_PushEnabled;
    }

    public static bool IsAutoPushPermissionCheckEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AutoPushPermissionCheck;
    }

    public static bool IsSocialEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_SocialEnabled;
    }

    public static bool IsCrashCaptureEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_CrashCaptureEnabled;
    }

    public static bool IsAnalyticsEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AnalyticsEnabled;
    }

    public static bool IsAutoTrackEventsEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AutoTrackEvents;
    }

    public static AttributionProvider GetAttributionProvider()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AttributionProvider;
    }

    public static bool IsAttributionStoreAccountAttributeEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AttributionStoreAccountAttribute;
    }

    public static string GetAirBridgeAppName()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AirBridgeAppName;
    }

    public static string GetAirBridgeAppToken()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AirBridgeAppToken;
    }

    public static bool IsAirBridgeSandbox()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AirBridgeSandbox;
    }

    public static LoggingLevel GetLoggingLevel()
    {
        var settings = GetOrCreateSettings();
        return settings.m_LoggingLevel;
    }

    public static bool IsAddEntitlementsEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_AddEntitlements;
    }

    public static bool IsDebugModeEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_DebugModeEnabled;
    }

    public static string GetTermsOfServiceUrl()
    {
        var settings = GetOrCreateSettings();
        return settings.m_TermsOfServiceUrl;
    }

    public static string GetPrivacyPolicyUrl()
    {
        var settings = GetOrCreateSettings();
        return settings.m_PrivacyPolicyUrl;
    }
    
    // Overlay Menu Visibility Getters
    public static bool IsProfileSectionVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowProfileSection;
    }
    
    public static bool IsSettingsTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowSettingsTile;
    }
    
    public static bool IsSupportTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowSupportTile;
    }
    
    public static bool IsFriendsTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowFriendsTile;
    }
    
    public static bool IsChatTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowChatTile;
    }
    
    public static bool IsStoreTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowStoreTile;
    }
    
    public static bool IsRewardsTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowRewardsTile;
    }
    
    public static bool IsTasksTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowTasksTile;
    }
    
    public static bool IsLeaderboardTileVisible()
    {
        var settings = GetOrCreateSettings();
        return settings.m_ShowLeaderboardTile;
    }
}

// Register a SettingsProvider using IMGUI for the drawing framework:
static class PlaytoliaEditorSettingsIMGUIRegister
{
    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
        // First parameter is the path in the Settings window.
        // Second parameter is the scope of this setting: it only appears in the Project Settings window.
        var provider = new SettingsProvider("Project/PlaytoliaEditorIMGUISettings", SettingsScope.Project)
        {
            // By default the last token of the path is used as display name if no label is provided.
            label = "Playtolia Settings",
            // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
            guiHandler = (searchContext) =>
            {
                var settings = PlaytoliaEditorSettings.GetSerializedSettings();
                EditorGUILayout.Space();
                // Playtolia.Core
                EditorGUILayout.LabelField("General", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_GameId"), new GUIContent("Game ID"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_Headless"), new GUIContent("Headless"));
                EditorGUILayout.HelpBox(
                    "When enabled, PlaytoliaUI attach (e.g. from PlaytoliaGameObject) will not attach any views or controllers. Use for API-only flows.",
                    MessageType.Info);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(
                    settings.FindProperty("m_OverlayButtonDraggingEnabled"),
                    new GUIContent("Enable Overlay Button Dragging"));
                EditorGUILayout.HelpBox(
                    "Allows players to drag the floating Playtolia button between corners or push it into a screen edge to minimize it.",
                    MessageType.Info);
                EditorGUILayout.Space();
                
                // Custom Origin
                EditorGUILayout.PropertyField(settings.FindProperty("m_EnableCustomOrigin"),
                    new GUIContent("Use Custom Origin"));
                
                if (settings.FindProperty("m_EnableCustomOrigin").boolValue)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_CustomOrigin"),
                        new GUIContent("Custom Origin URL"));
                    EditorGUILayout.HelpBox(
                        "Custom origin overrides the default Playtolia API endpoint. Only use this for testing or if directed by Playtolia support. " +
                        "Incorrect URLs will prevent the SDK from functioning.",
                        MessageType.Warning);
                }
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox(
                    "In order to use Playtolia features you must enable individual modules in the Playtolia settings. Selected modules will be automatically initialized at runtime. " +
                    "You can also use the Playtolia API to dynamically enable, disable, or configure modules at runtime.",
                    MessageType.Warning);
                // Playtolia.Auth
                EditorGUILayout.LabelField("Authentication", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthEnabled"),
                    new GUIContent("Enable Authentication"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthRequired"),
                    new GUIContent("Authentication Required"));
                EditorGUILayout.HelpBox(
                    "If enabled, users must authenticate before accessing the game. If disabled, Playtolia will not display the authentication UI, but you can still use the Playtolia.Auth API to dynamically prompt authentication.",
                    MessageType.Info);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Authentication Providers", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthProviderGoogleEnabled"),
                    new GUIContent("Google Authentication"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthProviderFacebookEnabled"),
                    new GUIContent("Facebook Authentication"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthProviderAppleEnabled"),
                    new GUIContent("Apple Authentication"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthProviderDiscordEnabled"),
                    new GUIContent("Discord Authentication"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthProviderPlayAsGuestEnabled"),
                    new GUIContent("Guest Authentication"));
                EditorGUILayout.Space();

                // Legal
                EditorGUILayout.LabelField("Legal", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_TermsOfServiceUrl"),
                    new GUIContent("Terms of Service URL"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_PrivacyPolicyUrl"),
                    new GUIContent("Privacy Policy URL"));
                EditorGUILayout.HelpBox(
                    "Links shown on the sign-up screen next to the consent checkbox. " +
                    "Leave a field empty to render that label as plain, non-clickable text.",
                    MessageType.Info);
                EditorGUILayout.Space();

                // Playtolia.Ticketing
                EditorGUILayout.LabelField("Support Ticketing", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_TicketingEnabled"),
                    new GUIContent("Enable Ticketing"));
                EditorGUILayout.Space();

                // Playtolia.Billing
                EditorGUILayout.LabelField("Billing and Store", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_BillingEnabled"),
                    new GUIContent("Enable Billing"));

                // Playtolia.Push
                EditorGUILayout.LabelField("Push Notifications", EditorStyles.whiteLargeLabel);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("m_PushEnabled"),
                    new GUIContent("Enable Push Notifications"));
                bool pushToggleChanged = EditorGUI.EndChangeCheck();
                EditorGUILayout.HelpBox(
                    "Push notifications use Firebase Cloud Messaging (FCM). Enabling this defines PLAYTOLIA_FIREBASE " +
                    "and compiles in the Firebase integration, so the Firebase Unity SDK (with Cloud Messaging) and your " +
                    "Google config files must be present in the project. Leave it off if you don't use push — the SDK " +
                    "then compiles without any Firebase dependency. To install FCM, see the Firebase docs for Unity.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AutoPushPermissionCheck"),
                    new GUIContent("Auto Request Push Permission"));
                EditorGUILayout.HelpBox(
                    "If enabled, the SDK will automatically check and request push notification permission when initialized. " +
                    "If disabled, you must manually request permission using the Playtolia.Push API.", MessageType.Info);
                EditorGUILayout.Space();
                
                // Playtolia.Social
                
                EditorGUILayout.LabelField("Social (Friends)", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_SocialEnabled"),
                    new GUIContent("Enable Social Features"));
                EditorGUILayout.Space();

                // Crash Capture
                EditorGUILayout.LabelField("Crash Capture", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_CrashCaptureEnabled"),
                    new GUIContent("Enable Crash Capture"));
                EditorGUILayout.HelpBox(
                    "Crash Capture helps identify and debug crashes by capturing exception information and logs. " +
                    "When a crash occurs, the user will be prompted on the next launch to share the crash report.",
                    MessageType.Info);
                EditorGUILayout.Space();

                // Analytics
                EditorGUILayout.LabelField("Analytics", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AnalyticsEnabled"),
                    new GUIContent("Enable Analytics"));
                EditorGUILayout.HelpBox(
                    "Analytics uses Grain SDK to track user events and behavior. " +
                    "The Game ID is automatically used as the Grain tenant identifier.",
                    MessageType.Info);

                if (settings.FindProperty("m_AnalyticsEnabled").boolValue)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AutoTrackEvents"),
                        new GUIContent("Auto-Track Events"));
                    EditorGUILayout.HelpBox(
                        "When enabled, the SDK will automatically track key events such as login, logout, purchases, friend activity, and profile changes.",
                        MessageType.Info);
                }
                EditorGUILayout.Space();

                // Attribution
                EditorGUILayout.LabelField("Attribution", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AttributionProvider"),
                    new GUIContent("Attribution Provider"));
                EditorGUILayout.HelpBox(
                    "Marketing attribution provider.\n" +
                    "• None: attribution disabled.\n" +
                    "• System: native store attribution. On Android this reads the Google Play install " +
                    "referrer (UTM parameters, click/install timestamps) with no third-party SDK or token. " +
                    "No effect on iOS.\n" +
                    "• Airbridge: the Airbridge MDP SDK (requires the Airbridge SDK in your project plus an app name and token).",
                    MessageType.Info);

                var attributionProvider = (AttributionProvider)settings.FindProperty("m_AttributionProvider").enumValueIndex;
                if (attributionProvider != AttributionProvider.None)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AttributionStoreAccountAttribute"),
                        new GUIContent("Store as Account Attribute"));
                    EditorGUILayout.HelpBox(
                        "When enabled, resolved attribution (e.g. the install referrer) is saved as a player " +
                        "account attribute under the 'install_referrer' key once the player signs in.",
                        MessageType.Info);
                }
                if (attributionProvider == AttributionProvider.Airbridge)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AirBridgeAppName"),
                        new GUIContent("Airbridge App Name"));
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AirBridgeAppToken"),
                        new GUIContent("Airbridge App Token"));
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AirBridgeSandbox"),
                        new GUIContent("Sandbox Environment"));
                    EditorGUILayout.HelpBox(
                        "Airbridge attribution requires you to add the Airbridge SDK to your project (Android: 'io.airbridge:sdk-android'; iOS: 'pod Airbridge'). " +
                        "Attribution is skipped at runtime if the App Name or Token is empty.",
                        MessageType.Warning);
                }
                EditorGUILayout.Space();

                // Signing
                EditorGUILayout.LabelField("Signing", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AddEntitlements"),
                    new GUIContent("Add iOS Entitlements"));
                EditorGUILayout.HelpBox(
                    "If enabled, the Playtolia SDK will automatically add the necessary entitlements to your iOS project. " +
                    "This is required to use Sign in with Apple and push notifications, if you are signing your bundles automatically.",
                    MessageType.Info);
                EditorGUILayout.Space();

                // Overlay Menu
                EditorGUILayout.LabelField("Overlay Menu Visibility", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Control which tiles and sections are visible in the overlay menu. " +
                    "These settings allow you to customize the menu based on your game's needs.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowProfileSection"),
                    new GUIContent("Show Profile Section"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowSettingsTile"),
                    new GUIContent("Show Settings Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowSupportTile"),
                    new GUIContent("Show Support Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowFriendsTile"),
                    new GUIContent("Show Friends Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowChatTile"),
                    new GUIContent("Show Chat Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowStoreTile"),
                    new GUIContent("Show Store Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowRewardsTile"),
                    new GUIContent("Show Rewards Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowTasksTile"),
                    new GUIContent("Show Tasks Tile"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_ShowLeaderboardTile"),
                    new GUIContent("Show Leaderboard Tile"));
                EditorGUILayout.Space();
                
                // Advanced
                EditorGUILayout.LabelField("Advanced Settings", EditorStyles.whiteLargeLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("m_DebugModeEnabled"),
                    new GUIContent("Enable Debug Mode"));
                EditorGUILayout.HelpBox("Debug mode enables additional developer tools and overrides logging level.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_LoggingLevel"), new GUIContent("Logging Level"));
                EditorGUILayout.HelpBox(
                    "Set the logging level for Playtolia. Higher levels will log more information, which can be useful for debugging but may impact performance.",
                    MessageType.Info);
                EditorGUILayout.HelpBox(
                    "Setting the logging level to 'Debug' may leak sensitive information in the logs. Use with caution in production builds.",
                    MessageType.Warning);

                settings.ApplyModifiedPropertiesWithoutUndo();

                // Keep PLAYTOLIA_FIREBASE in sync with the Push toggle. Deferred so the
                // resulting recompile doesn't run in the middle of this GUI pass.
                if (pushToggleChanged)
                {
                    bool pushEnabled = settings.FindProperty("m_PushEnabled").boolValue;
                    EditorApplication.delayCall += () => PlaytoliaEditorSettings.SyncFirebaseDefine(pushEnabled);
                }
            },

            // Populate the search keywords to enable smart search filtering and label highlighting:
            keywords = new HashSet<string>(new[]
            {
                "Playtolia", "Settings", "Game ID", "Headless", "Origin", "Custom Origin", "Base URL", "URL",
                "Authentication", "Ticketing", "Auth Enabled", "Auth Required", "Google", "Facebook", "Auth",
                "Apple", "Discord", "Guest", "Ticketing", "Play As Guest", "Ticketing Enabled", "Signing",
                "Entitlements", "Debug", "Debug Mode", "Logging", "Push", "Notifications",
                "Auto Permission", "Overlay", "Menu", "Visibility", "Profile", "Support",
                "Friends", "Chat", "Store", "Rewards", "Tasks", "Leaderboard", "Tiles",
                "Crash", "Crash Capture", "Error Reporting", "Diagnostics",
                "Analytics", "Grain", "Tracking", "Events", "Auto-Track",
                "Attribution", "Install Referrer", "Referrer", "UTM", "Airbridge", "Provider", "MDP",
                "Legal", "Terms", "Terms of Service", "Privacy", "Privacy Policy", "ToS"
            })
        };

        return provider;
    }
}
