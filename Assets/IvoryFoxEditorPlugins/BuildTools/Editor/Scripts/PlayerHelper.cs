using System;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    public static class PlayerHelper
    {
        public static void SetPlayerSettings(BuildData data)
        {
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, data.packageId);
            PlayerSettings.productName = data.productName;
            PlayerSettings.companyName = data.companyName;
            PlayerSettings.bundleVersion = data.productVersion;
            PlayerSettings.Android.bundleVersionCode = Convert.ToInt32(data.productVersion);

            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
            
            PlayerSettings.Android.targetArchitectures = data.androidArchitecture;
            PlayerSettings.Android.targetSdkVersion = data.targetSdkVersions;
            PlayerSettings.Android.minSdkVersion = data.minAndroidSdkVersions;
            
            PlayerSettings.Android.renderOutsideSafeArea = false;
            PlayerSettings.Android.useCustomKeystore = data.useCustomKeystore;
            PlayerSettings.defaultInterfaceOrientation = data.screenOrientation;

            SetIcons(data.icon);
            
            if (data.useCustomKeystore)
            {
                string fullAppName = "STPN-" + data.taskNumber;
                string keystoreName = $"{Path.Combine("Assets", fullAppName)}.keystore";
                
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = keystoreName;
                PlayerSettings.Android.keystorePass = fullAppName;

                PlayerSettings.Android.keyaliasPass = fullAppName;
                PlayerSettings.Android.keyaliasName = fullAppName;
                
                if (!KeyGenerator.DetectKey(fullAppName))
                {
                    KeyGenerator.Create(PlayerSettings.companyName, PlayerSettings.Android.keyaliasName, PlayerSettings.Android.keyaliasName);
                }
            }
            
            EditorUserBuildSettings.androidBuildType = data.buildType;
            EditorUserBuildSettings.development = data.development;

           if (data.development) data.buildOptions = (BuildOptions.Development | BuildOptions.CompressWithLz4);
           else data.buildOptions = (BuildOptions.StrictMode | BuildOptions.CompressWithLz4HC);

        }

        public static void PrintPlayerSettings()
        {
            Debug.Log("PlayerSettings:");
            Debug.Log("------------------------------------------------");
            Debug.Log("applicationIdentifier" + PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
            Debug.Log("productName" + PlayerSettings.productName);
            Debug.Log("companyName" + PlayerSettings.companyName);
            Debug.Log("bundleVersion" + PlayerSettings.bundleVersion);
            Debug.Log("bundleVersionCode" + PlayerSettings.Android.bundleVersionCode);
            Debug.Log("showUnityLogo" + PlayerSettings.SplashScreen.showUnityLogo);
            Debug.Log("scriptingBackend" + PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android));
            Debug.Log("apiCompatibilityLevel" + PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Android));
            Debug.Log("targetArchitectures" + PlayerSettings.Android.targetArchitectures);
            Debug.Log("targetSdkVersion" + PlayerSettings.Android.targetSdkVersion);
            Debug.Log("minSdkVersion" + PlayerSettings.Android.minSdkVersion);
            Debug.Log("renderOutsideSafeArea" + PlayerSettings.Android.renderOutsideSafeArea);
            Debug.Log("useCustomKeystore" + PlayerSettings.Android.useCustomKeystore);
            Debug.Log("defaultInterfaceOrientation" + PlayerSettings.defaultInterfaceOrientation);
            Debug.Log("------------------------------------------------");
        }
        
        private static void SetIcons(Texture2D textures)
        {
            var platform = BuildTargetGroup.Android;
            var kind = AndroidPlatformIconKind.Legacy;

            var icons = PlayerSettings.GetPlatformIcons(platform, kind);
        
            foreach (var t in icons) t.SetTexture(textures);
    
            PlayerSettings.SetPlatformIcons(platform, kind, icons);
        }
    }
}