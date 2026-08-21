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
    [SerializeField] private Color m_OverlayButtonBackgroundColor = new Color32(17, 20, 22, 255);
    [SerializeField] private Object m_OverlayButtonIcon;
    [SerializeField] private Object m_OverlayButtonMinimizedIcon;
    [SerializeField] private Object m_OverlayButtonExpandedArtwork;
    [SerializeField] private Object m_OverlayButtonMinimizedArtwork;
    [SerializeField] private bool m_OverlayButtonArtworkFollowsEdge = true;
    [SerializeField] private bool m_OverlayButtonShowIcon = true;
    [SerializeField] private bool m_OverlayButtonShowMinimizedIcon = true;
    [SerializeField, Range(44f, 120f)] private float m_OverlayButtonSizeDp = 64f;
    [SerializeField, Range(24f, 240f)] private float m_OverlayButtonExpandedLengthDp = 64f;
    [SerializeField, Range(24f, 160f)] private float m_OverlayButtonExpandedThicknessDp = 64f;
    // Radii render as sliders capped at 80dp: the thickest allowed button is 160dp, and the
    // runtime clamps a radius to half the smaller dimension anyway.
    [SerializeField, Range(0f, 80f)] private float m_OverlayButtonCornerRadiusDp = 16f;
    [SerializeField, Range(24f, 240f)] private float m_OverlayButtonMinimizedLengthDp = 58f;
    [SerializeField, Range(24f, 160f)] private float m_OverlayButtonMinimizedThicknessDp = 36f;
    [SerializeField, Range(0f, 80f)] private float m_OverlayButtonMinimizedCornerRadiusDp = 29f;
    [SerializeField] private bool m_OverlayButtonRespectSafeArea = true;
    [SerializeField, Min(0f)] private float m_OverlayButtonSafeAreaPaddingDp = 12f;
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
            settings.m_OverlayButtonBackgroundColor = new Color32(17, 20, 22, 255);
            settings.m_OverlayButtonShowIcon = true;
            settings.m_OverlayButtonShowMinimizedIcon = true;
            settings.m_OverlayButtonArtworkFollowsEdge = true;
            settings.m_OverlayButtonSizeDp = 64f;
            settings.m_OverlayButtonExpandedLengthDp = 64f;
            settings.m_OverlayButtonExpandedThicknessDp = 64f;
            settings.m_OverlayButtonCornerRadiusDp = 16f;
            settings.m_OverlayButtonMinimizedLengthDp = 58f;
            settings.m_OverlayButtonMinimizedThicknessDp = 36f;
            settings.m_OverlayButtonMinimizedCornerRadiusDp = 29f;
            settings.m_OverlayButtonRespectSafeArea = true;
            settings.m_OverlayButtonSafeAreaPaddingDp = 12f;
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

    public static string GetOverlayButtonBackgroundColor()
    {
        var color = GetOrCreateSettings().m_OverlayButtonBackgroundColor;
        var rgba = ColorUtility.ToHtmlStringRGBA(color);
        return "#" + rgba.Substring(6, 2) + rgba.Substring(0, 6);
    }

    public static string GetOverlayButtonIconDataUri()
    {
        return AssetDataUri(GetOrCreateSettings().m_OverlayButtonIcon);
    }

    public static string GetOverlayButtonMinimizedIconDataUri()
    {
        return AssetDataUri(GetOrCreateSettings().m_OverlayButtonMinimizedIcon);
    }

    public static string GetOverlayButtonExpandedArtworkDataUri() =>
        AssetDataUri(GetOrCreateSettings().m_OverlayButtonExpandedArtwork);

    public static string GetOverlayButtonMinimizedArtworkDataUri() =>
        AssetDataUri(GetOrCreateSettings().m_OverlayButtonMinimizedArtwork);

    public static bool GetOverlayButtonArtworkFollowsEdge() =>
        GetOrCreateSettings().m_OverlayButtonArtworkFollowsEdge;

    public static bool GetOverlayButtonShowIcon() => GetOrCreateSettings().m_OverlayButtonShowIcon;
    public static bool GetOverlayButtonShowMinimizedIcon() => GetOrCreateSettings().m_OverlayButtonShowMinimizedIcon;

    public static float GetOverlayButtonSizeDp() => GetOrCreateSettings().m_OverlayButtonSizeDp;
    public static float GetOverlayButtonExpandedLengthDp()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonExpandedLengthDp > 0f
            ? settings.m_OverlayButtonExpandedLengthDp : settings.m_OverlayButtonSizeDp;
    }

    public static float GetOverlayButtonExpandedThicknessDp()
    {
        var settings = GetOrCreateSettings();
        return settings.m_OverlayButtonExpandedThicknessDp > 0f
            ? settings.m_OverlayButtonExpandedThicknessDp : settings.m_OverlayButtonSizeDp;
    }
    public static float GetOverlayButtonCornerRadiusDp() => GetOrCreateSettings().m_OverlayButtonCornerRadiusDp;
    public static float GetOverlayButtonMinimizedLengthDp() => GetOrCreateSettings().m_OverlayButtonMinimizedLengthDp;
    public static float GetOverlayButtonMinimizedThicknessDp() => GetOrCreateSettings().m_OverlayButtonMinimizedThicknessDp;
    public static float GetOverlayButtonMinimizedCornerRadiusDp() => GetOrCreateSettings().m_OverlayButtonMinimizedCornerRadiusDp;
    public static bool GetOverlayButtonRespectSafeArea() => GetOrCreateSettings().m_OverlayButtonRespectSafeArea;
    public static float GetOverlayButtonSafeAreaPaddingDp() => GetOrCreateSettings().m_OverlayButtonSafeAreaPaddingDp;

    private static string AssetDataUri(Object asset)
    {
        if (asset == null) return null;
        var path = AssetDatabase.GetAssetPath(asset);
        var extension = Path.GetExtension(path).ToLowerInvariant();
        if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".svg")
        {
            Debug.LogWarning("Playtolia overlay assets must be PNG, JPEG, or SVG files: " + path);
            return null;
        }
        var bytes = File.ReadAllBytes(path);
        var mime = extension == ".png" ? "image/png" :
            extension == ".svg" ? "image/svg+xml" : "image/jpeg";
        return "data:" + mime + ";base64," + System.Convert.ToBase64String(bytes);
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
    private enum SettingsPage
    {
        General,
        Overlay,
        Features,
        DataAndDiagnostics
    }

    // Wide enough for the longest label ("Enable Overlay Button Dragging").
    private const float k_LabelWidth = 230f;

    // Keeps value controls from stretching across the full width of the Project Settings
    // window, and keeps the help box text at a readable line length on wide displays.
    private const float k_ContentWidth = 720f;
    private const float k_LeftMargin = 4f;

    [SettingsProvider]
    public static SettingsProvider CreateGeneralSettingsProvider()
    {
        return CreateSettingsProvider("Project/Playtolia/General", "General", SettingsPage.General);
    }

    [SettingsProvider]
    public static SettingsProvider CreateOverlaySettingsProvider()
    {
        return CreateSettingsProvider("Project/Playtolia/Overlay", "Overlay", SettingsPage.Overlay);
    }

    [SettingsProvider]
    public static SettingsProvider CreateFeaturesSettingsProvider()
    {
        return CreateSettingsProvider("Project/Playtolia/Features", "Features", SettingsPage.Features);
    }

    [SettingsProvider]
    public static SettingsProvider CreateDataSettingsProvider()
    {
        return CreateSettingsProvider(
            "Project/Playtolia/Data & Diagnostics",
            "Data & Diagnostics",
            SettingsPage.DataAndDiagnostics);
    }

    private static SettingsProvider CreateSettingsProvider(string path, string label, SettingsPage page)
    {
        var provider = new SettingsProvider(path, SettingsScope.Project)
        {
            label = label,
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
                bool pushToggleChanged = false;

                if (page == SettingsPage.General)
                {
                // Playtolia.Core
                EditorGUILayout.LabelField("General", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "Game ID identifies your project with Playtolia; copy it from the dashboard. " +
                    "Headless makes PlaytoliaUI attach (e.g. from PlaytoliaGameObject) skip all views and controllers, for API-only flows.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_GameId"), new GUIContent("Game ID"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_Headless"), new GUIContent("Headless"));
                EditorGUILayout.Space();
                }

                if (page == SettingsPage.Overlay)
                {
                // Live preview first, so every slider below shows its effect without scrolling.
                EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
                DrawOverlayButtonPreview(settings);
                EditorGUILayout.Space();

                // Overlay Button
                EditorGUILayout.LabelField("Overlay Button", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(
                    "The floating Playtolia button that opens the overlay menu. " +
                    "Dragging lets players move it around the screen or push it into a screen edge to minimize it. " +
                    "Minimized Opacity is the background opacity of the notch it collapses into, clamped to 0..1. " +
                    "The normal and collapsed states can use custom PNG/SVG icons, sizes, and corner radii; " +
                    "a zero corner radius creates a sharp drawer. Safe-area placement uses the OS cutout and system-bar insets. " +
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
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonBackgroundColor"),
                    new GUIContent("Background Color"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonIcon"),
                    new GUIContent("Expanded Icon (PNG/SVG)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonShowIcon"),
                    new GUIContent("Show Expanded Icon"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonMinimizedIcon"),
                    new GUIContent("Collapsed Icon (PNG/SVG)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonShowMinimizedIcon"),
                    new GUIContent("Show Collapsed Icon"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonExpandedLengthDp"),
                    new GUIContent("Expanded Length Along Edge (dp)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonExpandedThicknessDp"),
                    new GUIContent("Expanded Depth From Edge (dp)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonCornerRadiusDp"),
                    new GUIContent("Expanded Corner Radius (dp)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonMinimizedLengthDp"),
                    new GUIContent("Collapsed Length (dp)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonMinimizedThicknessDp"),
                    new GUIContent("Collapsed Thickness (dp)"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonMinimizedCornerRadiusDp"),
                    new GUIContent("Collapsed Corner Radius (dp)"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Complete Custom Artwork", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(
                    "Optional PNG, JPEG, or SVG files here replace the complete generated button for that state. " +
                    "Artwork is authored horizontally with its fixed edge at the bottom: Length runs along the edge " +
                    "and Depth runs away from it. Follow Docked Edge rotates it automatically for every dock position without stretching.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonExpandedArtwork"),
                    new GUIContent("Expanded Artwork"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonMinimizedArtwork"),
                    new GUIContent("Collapsed Artwork"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonArtworkFollowsEdge"),
                    new GUIContent("Follow Docked Edge"));
                EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonRespectSafeArea"),
                    new GUIContent("Respect Device Safe Area"));
                if (settings.FindProperty("m_OverlayButtonRespectSafeArea").boolValue)
                {
                    EditorGUILayout.PropertyField(settings.FindProperty("m_OverlayButtonSafeAreaPaddingDp"),
                        new GUIContent("Safe Area Padding (dp)"));
                }
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
                }

                if (page == SettingsPage.General)
                {
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
                }

                if (page == SettingsPage.Features)
                {
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
                pushToggleChanged = EditorGUI.EndChangeCheck();
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
                }

                if (page == SettingsPage.DataAndDiagnostics)
                {
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
                }

                if (page == SettingsPage.General)
                {
                // Signing
                EditorGUILayout.LabelField("Signing", EditorStyles.whiteLargeLabel);
                EditorGUILayout.HelpBox(
                    "If enabled, the Playtolia SDK will automatically add the necessary entitlements to your iOS project. " +
                    "This is required to use Sign in with Apple and push notifications, if you are signing your bundles automatically.",
                    MessageType.Info);
                EditorGUILayout.PropertyField(settings.FindProperty("m_AddEntitlements"),
                    new GUIContent("Add iOS Entitlements"));
                EditorGUILayout.Space();
                }

                if (page == SettingsPage.Overlay)
                {
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
                }
                
                if (page == SettingsPage.General)
                {
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
                }

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

            keywords = GetKeywords(page)
        };

        return provider;
    }

    // Height of the mock screen strip each preview state is drawn against.
    private const float k_PreviewPanelHeight = 120f;

    // Draws the expanded and collapsed button states side by side, docked against the bottom
    // of a mock screen strip, using the same background color, dimensions, corner radii,
    // opacity, icons, and artwork overrides the runtime button is generated from.
    private static void DrawOverlayButtonPreview(SerializedObject settings)
    {
        float expandedLength = settings.FindProperty("m_OverlayButtonExpandedLengthDp").floatValue;
        float expandedThickness = settings.FindProperty("m_OverlayButtonExpandedThicknessDp").floatValue;
        float expandedRadius = settings.FindProperty("m_OverlayButtonCornerRadiusDp").floatValue;
        float minimizedLength = settings.FindProperty("m_OverlayButtonMinimizedLengthDp").floatValue;
        float minimizedThickness = settings.FindProperty("m_OverlayButtonMinimizedThicknessDp").floatValue;
        float minimizedRadius = settings.FindProperty("m_OverlayButtonMinimizedCornerRadiusDp").floatValue;
        float minimizedOpacity = Mathf.Clamp01(settings.FindProperty("m_OverlayButtonMinimizedOpacity").floatValue);
        Color background = settings.FindProperty("m_OverlayButtonBackgroundColor").colorValue;

        bool showExpandedIcon = settings.FindProperty("m_OverlayButtonShowIcon").boolValue;
        bool showMinimizedIcon = settings.FindProperty("m_OverlayButtonShowMinimizedIcon").boolValue;
        Texture expandedIcon =
            PreviewTexture(settings.FindProperty("m_OverlayButtonIcon").objectReferenceValue);
        Texture minimizedIcon =
            PreviewTexture(settings.FindProperty("m_OverlayButtonMinimizedIcon").objectReferenceValue);
        Texture expandedArtwork =
            PreviewTexture(settings.FindProperty("m_OverlayButtonExpandedArtwork").objectReferenceValue);
        Texture minimizedArtwork =
            PreviewTexture(settings.FindProperty("m_OverlayButtonMinimizedArtwork").objectReferenceValue);

        EditorGUILayout.BeginHorizontal();
        DrawPreviewPanel(
            "Expanded",
            expandedLength, expandedThickness,
            background, expandedRadius,
            docked: false,
            showIcon: showExpandedIcon,
            icon: expandedIcon,
            artwork: expandedArtwork);
        GUILayout.Space(6f);
        DrawPreviewPanel(
            "Collapsed",
            minimizedLength, minimizedThickness,
            new Color(background.r, background.g, background.b, background.a * minimizedOpacity),
            minimizedRadius,
            docked: true,
            showIcon: showMinimizedIcon,
            icon: minimizedIcon,
            artwork: minimizedArtwork);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField(
            "Buttons are shown docked against the bottom edge; SVG icons and artwork preview " +
            "only when Unity imports them as textures.",
            EditorStyles.miniLabel);
    }

    private static void DrawPreviewPanel(
        string title,
        float lengthDp,
        float thicknessDp,
        Color color,
        float radiusDp,
        bool docked,
        bool showIcon,
        Texture icon,
        Texture artwork)
    {
        Rect panel = GUILayoutUtility.GetRect(
            0f, k_PreviewPanelHeight, GUILayout.ExpandWidth(true));
        if (Event.current.type != EventType.Repaint) return;

        // A mock slice of game screen for the button to sit on.
        EditorGUI.DrawRect(panel, new Color(0.16f, 0.19f, 0.23f, 1f));
        GUI.Label(
            new Rect(panel.x + 6f, panel.y + 4f, panel.width - 12f, 16f),
            title, EditorStyles.miniBoldLabel);

        // 1dp = 1px, shrunk only when the configured size does not fit the panel.
        float scale = Mathf.Min(
            1f,
            (panel.width - 24f) / Mathf.Max(lengthDp, 1f),
            (panel.height - 40f) / Mathf.Max(thicknessDp, 1f));
        float width = lengthDp * scale;
        float height = thicknessDp * scale;
        float radius = Mathf.Min(radiusDp * scale, Mathf.Min(width, height) / 2f);

        // Expanded floats just off the edge; collapsed sits flush in it, so only the corners
        // facing the screen stay rounded, matching the notch the runtime squeezes into.
        var rect = new Rect(
            panel.x + (panel.width - width) / 2f,
            docked ? panel.yMax - height : panel.yMax - height - 12f,
            width, height);
        var radii = docked
            ? new Vector4(radius, radius, 0f, 0f)
            : new Vector4(radius, radius, radius, radius);

        if (artwork != null)
        {
            GUI.DrawTexture(rect, artwork, ScaleMode.StretchToFill, true, 0f, Color.white, Vector4.zero, radii);
            return;
        }

        // Mirror the runtime layering: background color, the subtle white sheen gradient the
        // button paints over it, and the faint white gradient border.
        GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f, color, Vector4.zero, radii);
        GUI.DrawTexture(rect, SheenGradientTexture(), ScaleMode.StretchToFill, true, 0f, Color.white, Vector4.zero, radii);
        GUI.DrawTexture(
            rect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0f,
            new Color(1f, 1f, 1f, 0.07f), Vector4.one * Mathf.Max(1f, 2f * scale), radii);

        // The show toggles hide the glyph entirely, defaults included, matching the runtime.
        if (!showIcon) return;

        if (docked)
        {
            if (icon != null)
            {
                float iconSize = Mathf.Min(width, height) * 0.58f;
                GUI.DrawTexture(CenteredRect(rect, iconSize), icon, ScaleMode.ScaleToFit, true);
            }
            else
            {
                DrawChevronUp(rect, scale);
            }
        }
        else
        {
            float iconSize = Mathf.Min(width, height) * 0.625f;
            var iconRect = CenteredRect(rect, iconSize);
            if (icon != null)
            {
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit, true);
            }
            else
            {
                // The runtime falls back to the white Playtolia logomark.
                GUI.DrawTexture(iconRect, LogomarkTexture(), ScaleMode.ScaleToFit, true);
            }
        }
    }

    private static Rect CenteredRect(Rect container, float size) => new Rect(
        container.x + (container.width - size) / 2f,
        container.y + (container.height - size) / 2f,
        size, size);

    // The collapsed notch's default glyph: a chevron pointing into the screen (up, since the
    // preview docks against the bottom edge), matching the runtime EdgeChevron.
    private static void DrawChevronUp(Rect rect, float scale)
    {
        float centerX = rect.x + rect.width / 2f;
        float centerY = rect.y + rect.height / 2f;
        float arm = 5.3f * scale;
        var previous = Handles.color;
        Handles.color = new Color(1f, 1f, 1f, 0.92f);
        Handles.DrawAAPolyLine(
            2.5f * scale,
            new Vector3(centerX - arm, centerY + arm * 0.55f),
            new Vector3(centerX, centerY - arm * 0.55f),
            new Vector3(centerX + arm, centerY + arm * 0.55f));
        Handles.color = previous;
    }

    private static Texture2D s_SheenGradient;

    // Approximates the runtime's active-state overlay: white fading from 9% at the top of the
    // button to 1% at the bottom.
    private static Texture2D SheenGradientTexture()
    {
        if (s_SheenGradient == null)
        {
            s_SheenGradient = new Texture2D(1, 64, TextureFormat.RGBA32, false)
            {
                hideFlags = HideFlags.HideAndDontSave,
                wrapMode = TextureWrapMode.Clamp,
            };
            for (int y = 0; y < 64; y++)
            {
                // Texture row 0 is the bottom; the sheen is strongest at the top.
                float alpha = Mathf.Lerp(0.008f, 0.094f, y / 63f);
                s_SheenGradient.SetPixel(0, y, new Color(1f, 1f, 1f, alpha));
            }
            s_SheenGradient.Apply();
        }
        return s_SheenGradient;
    }

    private static Texture2D s_Logomark;

    // The runtime's default expanded icon is the white Playtolia logomark
    // (core/src/commonMain/moko-resources/images/logomark.svg). The preview rasterizes the same
    // even-odd polygon pair so the default button looks like the real one.
    private static Texture2D LogomarkTexture()
    {
        if (s_Logomark != null) return s_Logomark;

        Vector2[] outer =
        {
            new Vector2(6.0808f, 5.5f), new Vector2(22.9603f, 5.5f),
            new Vector2(27.9351f, 9.4977f), new Vector2(29.9192f, 18.6185f),
            new Vector2(26.6619f, 22.6457f), new Vector2(15.0831f, 22.6457f),
            new Vector2(16.9785f, 31.5f), new Vector2(11.7665f, 31.5f),
        };
        Vector2[] inner =
        {
            new Vector2(12.27f, 9.7346f), new Vector2(12.3291f, 10.2381f),
            new Vector2(14.1356f, 18.4111f), new Vector2(23.4044f, 18.4111f),
            new Vector2(24.4409f, 17.1379f), new Vector2(23.1083f, 11.0081f),
            new Vector2(21.5092f, 9.7346f),
        };

        const int size = 128;
        const float viewWidth = 36f, viewHeight = 37f;
        const int samples = 2; // 2x2 supersampling smooths the diagonal strokes.
        s_Logomark = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            hideFlags = HideFlags.HideAndDontSave,
            wrapMode = TextureWrapMode.Clamp,
        };
        for (int py = 0; py < size; py++)
        {
            for (int px = 0; px < size; px++)
            {
                float coverage = 0f;
                for (int sy = 0; sy < samples; sy++)
                {
                    for (int sx = 0; sx < samples; sx++)
                    {
                        float x = (px + (sx + 0.5f) / samples) * viewWidth / size;
                        // SVG y grows downward; texture rows grow upward.
                        float y = (size - 1 - py + (sy + 0.5f) / samples) * viewHeight / size;
                        bool filled = InsidePolygon(outer, x, y) ^ InsidePolygon(inner, x, y);
                        if (filled) coverage += 1f / (samples * samples);
                    }
                }
                s_Logomark.SetPixel(px, py, new Color(1f, 1f, 1f, coverage));
            }
        }
        s_Logomark.Apply();
        return s_Logomark;
    }

    private static bool InsidePolygon(Vector2[] polygon, float x, float y)
    {
        bool inside = false;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if ((polygon[i].y > y) != (polygon[j].y > y) &&
                x < (polygon[j].x - polygon[i].x) * (y - polygon[i].y) /
                    (polygon[j].y - polygon[i].y) + polygon[i].x)
            {
                inside = !inside;
            }
        }
        return inside;
    }

    // The icon and artwork slots accept any Object so SVGs can be assigned; only assets Unity
    // imported as textures (PNG/JPEG, or SVG with a texture importer) can be drawn here.
    private static Texture PreviewTexture(Object asset)
    {
        if (asset == null) return null;
        if (asset is Texture texture) return texture;
        if (asset is Sprite sprite) return sprite.texture;
        return AssetPreview.GetAssetPreview(asset);
    }

    private static HashSet<string> GetKeywords(SettingsPage page)
    {
        var keywords = new HashSet<string> { "Playtolia", "Settings" };
        string[] pageKeywords;
        switch (page)
        {
            case SettingsPage.Overlay:
                pageKeywords = new[]
                {
                    "Overlay", "Button", "Icon", "Artwork", "PNG", "SVG", "Shape", "Color", "Size",
                    "Safe Area", "Snap", "Auto-Hide", "Menu", "Visibility", "Tiles", "Panel", "Preview"
                };
                break;
            case SettingsPage.Features:
                pageKeywords = new[]
                {
                    "Authentication", "Google", "Facebook", "Apple", "Discord", "Guest", "Legal",
                    "Terms", "Privacy", "Ticketing", "Billing", "Store", "Push", "Notifications", "Social"
                };
                break;
            case SettingsPage.DataAndDiagnostics:
                pageKeywords = new[]
                {
                    "Crash", "Diagnostics", "Analytics", "Grain", "Tracking", "Attribution",
                    "Install Referrer", "UTM", "Airbridge"
                };
                break;
            default:
                pageKeywords = new[]
                {
                    "General", "Game ID", "Headless", "Origin", "Signing", "Entitlements", "Debug", "Logging"
                };
                break;
        }

        foreach (var keyword in pageKeywords)
        {
            keywords.Add(keyword);
        }
        return keywords;
    }
}
