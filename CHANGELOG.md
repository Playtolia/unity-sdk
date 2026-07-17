# Changelog

All notable changes to the Playtolia Unity package are documented here.
The Unity package version tracks the `com.playtolia.sdk:core` library version.

## Playtolia Unity SDK 1.3.9

 This Playtolia Unity SDK release is based on:

 - Playtolia Core (:android) 1.3.9
 - Playtolia Core (:ios) 1.3.9

### Changes:

- **Playtolia Guard — device security scanning** — new `PlaytoliaSecurity` component detects root/jailbreak, cheat tools, hooking frameworks, overlay apps, memory editors, and piracy tools against a platform-specific blocklist, exposing detected threats with `critical`/`warning`/`info` severity. Scans on init and optionally on foreground; results optionally forwarded to Grain analytics as `pta.security_scan`. Enabled declaratively via `enableSecurity`
- **Marketing attribution — `PlaytoliaAttribution`** — install attribution, session and purchase event forwarding, and deferred deep link resolution. Selectable provider (`none` / `system` / `airbridge`): `system` resolves install attribution from the Google Play Install Referrer on Android with no third-party SDK or token (no-op on iOS), and `airbridge` integrates the Airbridge MDP SDK. Optional bridge to Grain user properties and persistence of the resolved attribution as a player account attribute. Enabled declaratively via `attributionProvider` (legacy `enableAttribution` still honored)
- **Send test push** — new `PlaytoliaNotifications.SendTestPush(title, body, data)` Unity binding (backed by the `/notification/test-push` endpoint) for verifying push delivery end-to-end
- **Local / scheduled notifications (core)** — schedule, cancel, and cancel-all local notifications backed by a persistent Room-stored queue, with automatic reschedule on device boot (Android) and cleanup of expired entries
- **Resilient push token registration** — push tokens are now cached and automatically registered once the user authenticates, so tokens arriving before login are no longer dropped
- **Proactive token refresh** — access tokens are refreshed once past their half-life (with retry-and-backoff on failure), and refreshed at the start of each component sync so downstream syncs use a valid token — reducing mid-session auth interruptions
- **Terms of Service & Privacy Policy links on sign-up** — configurable via declarative `termsOfServiceUrl` / `privacyPolicyUrl`; render as plain text when unset
- **UPM scoped-registry distribution** — the package is now published to a Playtolia scoped registry at `https://dist.playtolia.com/unity-sdk` (scope `com.playtolia`). Adding the registry in Package Manager surfaces the full version list and one-click **Update**, in addition to the existing UPM git URL and `.unitypackage` install paths
- **Package extracted into the SDK monorepo** — the Unity package (previously loose under Game-Test-Bench) now lives in the SDK repo at `unity/com.playtolia.sdk` and is published to `github.com/Playtolia/unity-sdk`; the runtime C# API moved under `Runtime/`
- Database schema migrated to version 11 (adds the `scheduled_notifications` table)
- Updated store/billing documentation for purchase result callbacks and API reference
