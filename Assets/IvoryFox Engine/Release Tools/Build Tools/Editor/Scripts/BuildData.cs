using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    [Serializable, CreateAssetMenu(fileName = "BuildData", menuName = "IvoryFox/Build Tools/Create BuildData")]
    public class BuildData : ScriptableObject
    {
        public string taskNumber;
        public string productName;
        public string packageId;
        public string companyName;
        public string productVersion;
        public UIOrientation screenOrientation;

        public Texture2D icon;

        public bool development;
        public bool useCustomKeystore;

        public ScriptingImplementation scriptingImplementation;
        public AndroidArchitecture androidArchitecture;
        public AndroidSdkVersions minAndroidSdkVersions;
        public AndroidSdkVersions targetSdkVersions;

        public Il2CppCompilerConfiguration compilerConfiguration;
        public AndroidBuildType buildType;
        public BuildOptions buildOptions;
    
        public bool IsAllDataInput()
        {
            List<string> a = new List<string>()
            {
                taskNumber, companyName, productName, packageId, companyName, productVersion
            };
        
            return !a.Any(string.IsNullOrEmpty);
        }
        
        public string GetApkName => $"V{PlayerSettings.bundleVersion}_{taskNumber}_{(development ? "test" : "release")}.apk";
    }
}