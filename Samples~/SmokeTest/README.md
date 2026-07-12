# Smoke Test

A minimal, dependency-light verification that the native bridge is reachable.

## Use

1. Import via **Package Manager ▸ Playtolia SDK ▸ Samples ▸ Smoke Test ▸ Import**.
2. Create an empty scene, add a GameObject, attach `PlaytoliaSmokeTest`.
3. Add a `PlaytoliaGameObject` to the scene so the SDK boots.
4. Build to an Android device or iOS device and watch the log for
   `[PlaytoliaSmokeTest] PASS`.

CI imports this sample and compiles it for the Android and iOS build targets as
the package's smoke gate — if the C# API, the `[DllImport]` signatures, or the
asmdef wiring drift, the sample fails to compile and the build goes red.
