using System.IO;
using UnityEditor;
using UnityEngine;
using WebSdk.SdkConfigurations.RemoteConfigBased.Runtime.Scripts;

namespace WebSdk.SdkConfigurations.RemoteConfigBased.Editor.Scripts
{
    public class RemoteConfigBasedFactoryHelper: MonoBehaviour
    {
        [MenuItem("IvoryFox/WebSdk/Create RemoteConfigBasedFactoryAsset")]
        public static void RemoteConfigBasedFactoryAsset()
        {
            var repositoryData = Resources.Load<RemoteConfigBasedFactory>("WebSdkComponentsFactory");

            if (repositoryData is null)
            {
                string packageManagerFolder = Application.dataPath + "/PackageManagerAssets/Resources";
                string repositoryDataPath = "Assets/PackageManagerAssets/Resources/WebSdkComponentsFactory.asset";

                if (!Directory.Exists(packageManagerFolder)) Directory.CreateDirectory(packageManagerFolder);
                
                repositoryData = ScriptableObject.CreateInstance<RemoteConfigBasedFactory>();
                AssetDatabase.CreateAsset(repositoryData, repositoryDataPath);
                AssetDatabase.SaveAssets();
            }
        }
    }
}