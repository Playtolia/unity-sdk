using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Playtolia.Editor
{
    /// <summary>
    /// Windows player post-build: writes the game's Playtolia configuration where the desktop host
    /// reads it (&lt;Product&gt;_Data/Playtolia/playtolia_conf.json) and puts Skia's ICU data
    /// (icudtl.dat, needed for text layout) next to the executable.
    /// </summary>
    public class PlaytoliaDesktopBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.StandaloneWindows64) return;

            var exePath = report.summary.outputPath;
            var buildDir = Path.GetDirectoryName(exePath);
            var dataDir = Path.Combine(buildDir, Path.GetFileNameWithoutExtension(exePath) + "_Data");

            var configDir = Path.Combine(dataDir, "Playtolia");
            Directory.CreateDirectory(configDir);
            File.WriteAllText(
                Path.Combine(configDir, "playtolia_conf.json"),
                new PlaytoliaConfigurationHelper().SerializeSettings()
            );

            var package = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(PlaytoliaDesktopBuild).Assembly);
            var icuSource = package == null
                ? null
                : Path.Combine(package.resolvedPath, "Plugins", "Windows", "IcuData~", "icudtl.dat");
            if (icuSource != null && File.Exists(icuSource))
            {
                File.Copy(icuSource, Path.Combine(buildDir, "icudtl.dat"), true);
            }
            else
            {
                Debug.LogWarning("Playtolia: icudtl.dat not found in the package (Plugins/Windows/IcuData~); " +
                                 "text in the SDK UI may not lay out correctly. Rebuild the desktop library.");
            }
        }
    }

    /// <summary>The desktop library and its MinGW runtime are Windows x64 player plugins only.</summary>
    public class PlaytoliaDesktopPluginImporter : AssetPostprocessor
    {
        private const string WindowsPluginDir = "Packages/com.playtolia.sdk/Plugins/Windows/x86_64/";

        private void OnPreprocessAsset()
        {
            if (!assetPath.StartsWith(WindowsPluginDir) || !assetPath.EndsWith(".dll")) return;
            if (!(assetImporter is PluginImporter importer)) return;

            importer.SetCompatibleWithAnyPlatform(false);
            importer.SetCompatibleWithEditor(false);
            importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, true);
            importer.SetPlatformData(BuildTarget.StandaloneWindows64, "CPU", "x86_64");
            importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
            importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
            importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
            importer.SetCompatibleWithPlatform(BuildTarget.iOS, false);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, false);
        }
    }
}
