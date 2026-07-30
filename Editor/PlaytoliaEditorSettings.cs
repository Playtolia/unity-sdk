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

// New modes are appended so the values already serialized in a project's settings asset keep
// the numbers they were saved with.
enum OverlayButtonSnapModeSetting
{
    Corner,
    EdgeLeftRight,
    Free,
    EdgeTopBottom,
    EdgeAny
}

enum OverlayMenuPresentationSetting
{
    BottomSheet,
    LeftPanel
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
    [SerializeField] private OverlayButtonSnapModeSetting m_OverlayButtonSnapMode = OverlayButtonSnapModeSetting.EdgeLeftRight;
    [SerializeField] private bool m_OverlayButtonAutoHideEnabled;
    [SerializeField] private int m_OverlayButtonAutoHideDelayMilliseconds = 6000;
    [SerializeField] private float m_OverlayButtonMinimizedOpacity = 0.72f;
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
    
    // Overlay Menu
    [SerializeField] private OverlayMenuPresentationSetting m_OverlayMenuPresentation = OverlayMenuPresentationSetting.BottomSheet;
    [SerializeField] private float m_OverlayMenuPanelWidthFraction = 0.5f;

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
            settings.m_OverlayButtonSnapMode = OverlayButtonSnapModeSetting.EdgeLeftRight;
            settings.m_OverlayButtonAutoHideEnabled = false;
            settings.m_OverlayButtonAutoHideDelayMilliseconds = 6000;
            settings.m_OverlayButtonMinimizedOpacity = 0.72f;
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
            
            // Overlay Menu
            settings.m_OverlayMenuPresentation = OverlayMenuPresentationSetting.BottomSheet;
            settings.m_OverlayMenuPanelWidthFraction = 0.5f;

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

    public static string GetOverlayButtonSnapMode()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonSnapMode.ToString().ToUpperInvariant();
    }

    public static bool IsOverlayButtonAutoHideEnabled()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonAutoHideEnabled;
    }

    public static int GetOverlayButtonAutoHideDelayMilliseconds()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonAutoHideDelayMilliseconds;
    }

    public static float GetOverlayButtonMinimizedOpacity()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonMinimizedOpacity;
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
    
    public static string GetOverlayMenuPresentation()
    {
        var settings = GetOrCreateSettings();
        // BottomSheet becomes BOTTOM_SHEET, which is what the native parser expects.
        return System.Text.RegularExpressions.Regex.Replace(
            settings.m_OverlayMenuPresentation.ToString(), "([a-z])([A-Z])", "$1_$2")
            .ToUpperInvariant();
    }

    public static float GetOverlayMenuPanelWidthFraction()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayMenuPanelWidthFraction;
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
    // Wide enough for the longest label ("Enable Overlay Button Dragging").
    private const float k_LabelWidth = 230f;

    // Keeps value controls from stretching across the full width of the Project Settings
    // window, and keeps the help box text at a readable line length on wide displays.
    private const float k_ContentWidth = 720f;
    private const float k_LeftMargin = 4f;

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

                // The default label width (~150px) truncates labels like "Enable Overlay Button
                // Dragging", while the value controls stretch across the whole window. Widen the
                // labels and cap the content width so the page reads like a normal inspector.
                float previousLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = k_LabelWidth;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(k_LeftMargin);
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(k_ContentWidth));

                EditorGUILayout.Space();
                // Playtolia.Core
                EditorGUILayout.LabelField("General", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Game ID identifies your project with Playtolia; copy it from the dashboard. " +
                    "Headless makes PlaytoliaUI attach (e.g. from PlaytoliaGameObject) skip all views and controllers, for API-only flows.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_GameId"), new GUIContent("Game ID"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_Headless"), new GUIContent("Headless"));
                EditorGUILayout.Space();

                // Overlay Button
                EditorGUILayout.LabelField("Overlay Button", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(
                    "The floating Playtolia button that opens the overlay menu. " +
                    "Dragging lets players move it around the screen or push it into a screen edge to minimize it. " +
                    "Minimized Opacity is the background opacity of the notch it collapses into, clamped to 0..1. " +
                    "Auto-Hide minimizes the button against its current side after a period without interaction; " +
                    "players bring it back by tapping or swiping the remaining notch.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(
                    settings.FindProperty("m_OverlayButtonDraggingEnabled"),
                    new GUIContent("Enable Overlay Button Dragging"));
                EditorGUILayout.HelpBox(
                    "Where the button settles when the player lets go. Each edge mode is named " +
                    "after the edges it allows, and pushing the button past one of them minimizes it.\n" +
                    "• Edge Left Right: keeps the height it was released at and hugs the nearest side.\n" +
                    "• Edge Top Bottom: keeps its horizontal position and hugs the top or bottom.\n" +
                    "• Edge Any: hugs whichever of the four edges is nearest.\n" +
                    "• Corner: snaps to the nearest of the four corners.\n" +
                    "• Free: stays exactly where released. Pushing it into a side edge still minimizes it.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(
                    settings.FindProperty("m_OverlayButtonSnapMode"),
                    new GUIContent("Overlay Button Snap Mode"));
                EditorGUILayout.PropertyField(
                    settings.FindProperty("m_OverlayButtonMinimizedOpacity"),
                    new GUIContent("Minimized Opacity"));
                EditorGUILayout.PropertyField(
                    settings.FindProperty("m_OverlayButtonAutoHideEnabled"),
                    new GUIContent("Auto-Hide Overlay Button"));
                if (settings.FindProperty("m_OverlayButtonAutoHideEnabled").boolValue)
                {
                    EditorGUILayout.PropertyField(
                        settings.FindProperty("m_OverlayButtonAutoHideDelayMilliseconds"),
                        new GUIContent("Auto-Hide Delay (ms)"));
                }
                EditorGUILayout.Space();

                // Custom Origin
                EditorGUILayout.LabelField("Custom Origin", EditorStyles.boldLabel);
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
                EditorGUILayout.HelpBox(
                    "Authentication Required gates the game behind sign-in. With it off, Playtolia does not display the " +
                    "authentication UI, but you can still use the Playtolia.Auth API to prompt authentication at any time. " +
                    "The providers below are the sign-in options offered on the authentication screen.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthEnabled"),
                    new GUIContent("Enable Authentication"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_AuthRequired"),
                    new GUIContent("Authentication Required"));
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
                EditorGUILayout.HelpBox(
                    "Links shown on the sign-up screen next to the consent checkbox. " +
                    "Leave a field empty to render that label as plain, non-clickable text.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_TermsOfServiceUrl"),
                    new GUIContent("Terms of Service URL"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_PrivacyPolicyUrl"),
                    new GUIContent("Privacy Policy URL"));
                EditorGUILayout.Space();

                // Playtolia.Ticketing
                EditorGUILayout.LabelField("Support Ticketing", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Lets players open support tickets and chat with your team from the overlay menu. " +
                    "Tickets are managed from the Playtolia dashboard.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_TicketingEnabled"),
                    new GUIContent("Enable Ticketing"));
                EditorGUILayout.Space();

                // Playtolia.Billing
                EditorGUILayout.LabelField("Billing and Store", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "In-app purchases and the in-game store, including wallet currencies, entitlements, and subscriptions. " +
                    "Products and prices are configured on the Playtolia dashboard.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_BillingEnabled"),
                    new GUIContent("Enable Billing"));
                EditorGUILayout.Space();

                // Playtolia.Push
                EditorGUILayout.LabelField("Push Notifications", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Push notifications use Firebase Cloud Messaging (FCM). Enabling this defines PLAYTOLIA_FIREBASE " +
                    "and compiles in the Firebase integration, so the Firebase Unity SDK (with Cloud Messaging) and your " +
                    "Google config files must be present in the project. Leave it off if you don't use push, and the SDK " +
                    "compiles without any Firebase dependency. Auto Request Push Permission asks for the notification " +
                    "permission on initialization; with it off you request permission yourself via the Playtolia.Push API.",
                    MessageType.Info);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("m_PushEnabled"),
                    new GUIContent("Enable Push Notifications"));
                bool pushToggleChanged = EditorGUI.EndChangeCheck();
                EditorGUILayout.PropertyField(settings.FindProperty("m_AutoPushPermissionCheck"),
                    new GUIContent("Auto Request Push Permission"));
                EditorGUILayout.Space();

                // Playtolia.Social
                EditorGUILayout.LabelField("Social (Friends)", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Friends, friend requests, and player search, surfaced through the Friends and Chat tiles of the overlay menu.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_SocialEnabled"),
                    new GUIContent("Enable Social Features"));
                EditorGUILayout.Space();

                // Crash Capture
                EditorGUILayout.LabelField("Crash Capture", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Crash Capture helps identify and debug crashes by capturing exception information and logs. " +
                    "When a crash occurs, the user will be prompted on the next launch to share the crash report.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_CrashCaptureEnabled"),
                    new GUIContent("Enable Crash Capture"));
                EditorGUILayout.Space();

                // Analytics
                EditorGUILayout.LabelField("Analytics", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Analytics uses Grain SDK to track user events and behavior, with the Game ID as the Grain tenant identifier. " +
                    "Auto-Track Events additionally records key events such as login, logout, purchases, friend activity, and profile changes " +
                    "without any calls from your game.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AnalyticsEnabled"),
                    new GUIContent("Enable Analytics"));
                if (settings.FindProperty("m_AnalyticsEnabled").boolValue)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AutoTrackEvents"),
                        new GUIContent("Auto-Track Events"));
                }
                EditorGUILayout.Space();

                // Attribution
                EditorGUILayout.LabelField("Attribution", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Marketing attribution provider.\n" +
                    "• None: attribution disabled.\n" +
                    "• System: native store attribution. On Android this reads the Google Play install " +
                    "referrer (UTM parameters, click/install timestamps) with no third-party SDK or token. " +
                    "No effect on iOS.\n" +
                    "• Airbridge: the Airbridge MDP SDK (requires the Airbridge SDK in your project plus an app name and token).\n" +
                    "Store as Account Attribute saves the resolved attribution under the 'install_referrer' key " +
                    "on the player account once the player signs in.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AttributionProvider"),
                    new GUIContent("Attribution Provider"));

                var attributionProvider = (AttributionProvider)settings.FindProperty("m_AttributionProvider").enumValueIndex;
                if (attributionProvider != AttributionProvider.None)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_AttributionStoreAccountAttribute"),
                        new GUIContent("Store as Account Attribute"));
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
                EditorGUILayout.HelpBox(
                    "If enabled, the Playtolia SDK will automatically add the necessary entitlements to your iOS project. " +
                    "This is required to use Sign in with Apple and push notifications, if you are signing your bundles automatically.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AddEntitlements"),
                    new GUIContent("Add iOS Entitlements"));
                EditorGUILayout.Space();

                // Overlay Menu
                EditorGUILayout.LabelField("Overlay Menu", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "How the overlay menu and the screens opened from it are presented.\n" +
                    "• Bottom Sheet: slides up from the bottom of the screen.\n" +
                    "• Left Panel: slides in from the leading screen edge as a side panel. Its Panel Width Fraction " +
                    "is the share of the screen it covers (0.5 is half), clamped to 0.2..1.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayMenuPresentation"),
                    new GUIContent("Menu Presentation"));
                if ((OverlayMenuPresentationSetting)settings.FindProperty("m_OverlayMenuPresentation").enumValueIndex
                    == OverlayMenuPresentationSetting.LeftPanel)
                {
                    EditorGUILayout.PropertyField(
                        settings.FindProperty("m_OverlayMenuPanelWidthFraction"),
                        new GUIContent("Panel Width Fraction"));
                }
                EditorGUILayout.Space();

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
                EditorGUILayout.HelpBox(
                    "Debug Mode enables additional developer tools and overrides the logging level. " +
                    "Logging Level controls how much Playtolia writes to the log: higher levels are useful for debugging but may impact performance.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_DebugModeEnabled"),
                    new GUIContent("Enable Debug Mode"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_LoggingLevel"), new GUIContent("Logging Level"));
                EditorGUILayout.HelpBox(
                    "Setting the logging level to 'Debug' may leak sensitive information in the logs. Use with caution in production builds.",
                    MessageType.Warning);
                EditorGUILayout.Space();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUIUtility.labelWidth = previousLabelWidth;

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
                "Overlay Button", "Snap", "Snap Mode", "Auto-Hide", "Auto Hide", "Minimized",
                "Opacity", "Dragging", "Position",
                "Presentation", "Bottom Sheet", "Side Panel", "Left Panel", "Panel Width",
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
