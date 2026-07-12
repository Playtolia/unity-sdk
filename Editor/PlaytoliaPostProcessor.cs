using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Extensions;

namespace Playtolia.Editor
{
    public static class PlaytoliaPostProcessor
    {
        static string template =
            "{\n  \"gameId\": \"@@@co.xreos.playtoliasdk.platform.BuildConstants.GAME_ID@@@\",\n  \"debug\": true,\n  \"enablePersistence\": true\n}";
        
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            switch (buildTarget)
            {
                case BuildTarget.iOS:
                    PostProcessIos(buildTarget, pathToBuiltProject);
                    break;
                case BuildTarget.Android:
                    PostProcessAndroid(buildTarget, pathToBuiltProject);
                    break;
            }
            
        }
        
        private static void PostProcessAndroid(BuildTarget buildTarget, string pathToBuiltProject)
        {
            Debug.Log("Post-processing Android build for Playtolia SDK: " + pathToBuiltProject);
        }
        
        private static void PostProcessIos(BuildTarget buildTarget, string pathToBuiltProject)
        {
            Debug.Log("Post-processing iOS build for Playtolia SDK: " + pathToBuiltProject);
            var frameworkConfigPath = pathToBuiltProject + "/Frameworks/PlaytoliaSDK/Plugins/iOS/core.framework/com.playtolia.sdk-core.bundle/Contents/Resources/files/playtolia_conf.json";
            
            // Check if the shared framework exists
            if (!System.IO.File.Exists(frameworkConfigPath))
            {
                throw new Exception("The shared Playtolia framework does not exist at the specified path: " + frameworkConfigPath);
            }
            
            string conf = (new PlaytoliaConfigurationHelper()).SerializeSettings();
            
            // Write the modified contents back to the shared framework
            System.IO.File.WriteAllText(frameworkConfigPath, conf);
            
            // Add Sign in with Apple and In-App Purchase entitlements
            AddEntitlementsIfNecessary(buildTarget, pathToBuiltProject);
        }
        
        private static void AddEntitlementsIfNecessary(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (!PlaytoliaEditorSettings.IsAddEntitlementsEnabled()) return;
            
            // Initialize PBXProject
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            // Get Main target GUID
            string mainTargetGuid = pbxProject.GetUnityMainTargetGuid();

            
            // Check if there's already an entitlements file created and use it. If not, create a new file called ios.entitlements
            string entitlementsFile = pbxProject.GetBuildPropertyForAnyConfig(mainTargetGuid, "CODE_SIGN_ENTITLEMENTS");
            if (entitlementsFile == null)
            {
                entitlementsFile = string.Format("ios.entitlements");
            }

            // Initialize ProjectCapabilityManager and add 'Sign In With Apple' capability
            ProjectCapabilityManager capabilityManager = new ProjectCapabilityManager(projectPath, entitlementsFile, targetGuid: mainTargetGuid);
            capabilityManager.AddSignInWithApple();
            capabilityManager.AddInAppPurchase();

            // Call WriteToFile to save the changes to project file
            capabilityManager.WriteToFile();

        }
    }
}