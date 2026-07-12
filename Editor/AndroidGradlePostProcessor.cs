using UnityEngine;

using UnityEditor.Android;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System;
using System.Text.RegularExpressions;
using Playtolia.Editor;

public class AndroidGradlePostProcessor : IPostGenerateGradleAndroidProject
{
    private const string TargetAgp = "8.9.1";              // <- your required AGP
    private const string TargetGradle = "8.11.1";            // <- Gradle that matches that AGP
    private const string TargetKotlin = "1.9.25"; // if you use Kotlin; otherwise harmless
    private const string TargetGradleSha256   = "89d4e70e4e84e2d2dfbb63e4daa53e21b25017cc70c37e4eea31ee51fb15098a"; // REQUIRED with Gradle 8.8+
    
    public int callbackOrder => 15;      // after EDM

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("Post-processing Android build for Playtolia SDK: " + path);
        
        // 1. Add Playtolia configuration
        AddPlaytoliaConfiguration(path);
        
        var root = Directory.GetParent(path)!.FullName;
        //PatchRootBuildGradle(Path.Combine(root, "build.gradle"));
        //PatchGradleWrapper(Path.Combine(root, "gradle", "wrapper", "gradle-wrapper.properties"));
    }
    
    public void AddPlaytoliaConfiguration(string path)
    {
        // 2. Emit res/xml/auth_config.xml
        var resDir = Path.Combine(path, "src", "main", "res", "raw");
        Directory.CreateDirectory(resDir);
        var gameIdBuildConstTag = "@@@co.xreos.playtoliasdk.platform.BuildConstants.GAME_ID@@@";
            
        // Replace the game ID placeholder with the actual game ID
        string gameId = PlaytoliaEditorSettings.GetGameId();
            
        if (gameId == null)
        {
            throw new Exception("Game ID is not set in PlaytoliaEditorSettings.");
        }

        string conf = (new PlaytoliaConfigurationHelper()).SerializeSettings();
        
        System.IO.File.WriteAllText(Path.Combine(resDir, "playtolia_conf_json.json"), conf);
    }
    
    private void PatchRootBuildGradle(string file)
    {
        var text = File.ReadAllText(file);

        // Force AGP (com.android.tools.build:gradle)
        text = Regex.Replace(
            text,
            @"classpath\s+['""]com\.android\.tools\.build:gradle:[^'""]+['""]",
            $"classpath 'com.android.tools.build:gradle:{TargetAgp}'");
        
        // For AGP (com.android.application or com.android.library)
        var agpPattern = @"(?<=id\s+['""]com\.android\.(?:application|library)['""]\s+version\s+['""])([^'""]+)(?=['""])";
        text = Regex.Replace(text, agpPattern, TargetAgp, RegexOptions.Multiline);

        // (Optional) Kotlin Android plugin version, if present
        var kotlinPattern = @"(?<=id\s+['""]org\.jetbrains\.kotlin\.android['""]\s+version\s+['""])([^'""]+)(?=['""])";
        text = Regex.Replace(text, kotlinPattern, TargetKotlin, RegexOptions.Multiline);
        
        Debug.Log($"RootBuildGradle:");
        Debug.Log(text);
        Debug.Log($"AGP patched to {TargetAgp} in {file}");

        // (Optional) bump kotlin plugin if present
        text = Regex.Replace(
            text,
            @"classpath\s+['""]org\.jetbrains\.kotlin:kotlin-gradle-plugin:[^'""]+['""]",
            $"classpath 'org.jetbrains.kotlin:kotlin-gradle-plugin:{TargetKotlin}'");

        File.WriteAllText(file, text);
        Debug.Log($"AGP patched to {TargetAgp} in {file}");
    }

    private void PatchGradleWrapper(string file)
    {
        var text = File.ReadAllText(file);
        text = Regex.Replace(
            text,
            @"distributionUrl=.*",
            $"distributionUrl=https\\://services.gradle.org/distributions/gradle-{TargetGradle}-all.zip");
        File.WriteAllText(file, text);
        Debug.Log($"Gradle wrapper patched to {TargetGradle} in {file}");
    }
}