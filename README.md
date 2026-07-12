# Playtolia SDK for Unity

Unity client package for the Playtolia platform (authentication, billing, store,
wallet, entitlements, subscriptions, social, push, ticketing, party, analytics).

This package wraps the Kotlin Multiplatform `com.playtolia.sdk:core` library and
exposes it to Unity through C# on both **Android** and **iOS**.

## Installation

### UPM (git, recommended)

Add to your project's `Packages/manifest.json` (replace `vX.Y.Z` with the latest
[release](https://github.com/Playtolia/unity-sdk/releases)):

```json
"com.playtolia.sdk": "https://github.com/Playtolia/unity-sdk.git#vX.Y.Z"
```

Or in Unity: **Window ▸ Package Manager ▸ + ▸ Add package from git URL…** and paste
`https://github.com/Playtolia/unity-sdk.git#vX.Y.Z`.

Pin to a tag (`#vX.Y.Z`) for reproducible builds — git URLs have no version ranges, and the
package's C#, iOS framework, and Android Maven version move together. Omitting the fragment
resolves `main` once and locks that commit in `packages-lock.json` (it won't auto-update).

### `.unitypackage`

Download `PlaytoliaSDK-vX.Y.Z.unitypackage` from the
[Releases](https://github.com/Playtolia/unity-sdk/releases) page and drag it into
your project. Prefer UPM where possible — `.unitypackage` installs are unversioned
and harder to update.

## Native dependencies

- **Android** — resolved automatically from Maven Central via External Dependency
  Manager (EDM4U). The package ships `Editor/SharedAndroidDependencies.xml` pinned
  to the matching `com.playtolia.sdk:core-android` version. EDM4U is a transitive
  requirement; install `com.google.external-dependency-manager` if your project
  doesn't already have it.
- **iOS** — the `core.framework` binary ships inside `Plugins/iOS/` and is embedded
  automatically by the included post-processor.

## Requirements

- Unity 6000.0.32f1+
- Android min SDK 24
- iOS deployment target 13.0+

## Quick start

```csharp
using PlaytoliaSDK.Runtime;

PlaytoliaGameObject.Initialize();        // boot the SDK
PlaytoliaAuth.SerializeState();          // inspect current auth state
```

See the **Smoke Test** sample (Package Manager ▸ Samples) for a minimal
end-to-end example.

## Versioning

The package version tracks the `com.playtolia.sdk:core` library version exactly.
Package `X.Y.Z` always pairs with core `X.Y.Z`.
