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
        public BuildVersion buildVersion;
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
            List<string> a = new List<string>{taskNumber, companyName, productName, packageId, companyName, productVersion};
            return !a.Any(string.IsNullOrEmpty);
        }

        public string GetApkName(bool next = false)
        {
            string build = next ? buildVersion.GetNextBuildVersionAsString() : buildVersion.GetBuildVersionAsString();
            return $"V{PlayerSettings.bundleVersion}_{taskNumber}_{(development ? "test" : "release")}_{build}.apk";
        }

        public void Copy(BuildData newBuildData)
        {
            newBuildData.buildVersion = buildVersion;
            newBuildData.taskNumber = taskNumber;
            newBuildData.productName = productName;
            newBuildData.packageId = packageId;
            newBuildData.companyName = companyName;
            newBuildData.productVersion = productVersion;
            newBuildData.screenOrientation = screenOrientation;
            newBuildData.icon = icon;
            newBuildData.development = development;
            newBuildData.useCustomKeystore = useCustomKeystore;
        }
    }
}