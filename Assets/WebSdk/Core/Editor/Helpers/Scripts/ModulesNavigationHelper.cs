using System.IO;
using UnityEditor;
using UnityEngine;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;

namespace WebSdk.Core.Editor.Helpers.Scripts
{
    public static class ModulesNavigationHelper
    {
        [MenuItem("IvoryFox/WebSdk/Create ModulesNavigationAsset")]
        public static void CreateModulesNavigationAsset()
        {
            var repositoryData = Resources.Load<ModulesNavigationData>("ModulesNavigation");

            if (repositoryData is null)
            {
                string packageManagerFolder = Application.dataPath + "/PackageManagerAssets/Resources";
                string repositoryDataPath = "Assets/PackageManagerAssets/Resources/ModulesNavigation.asset";

                if (!Directory.Exists(packageManagerFolder)) Directory.CreateDirectory(packageManagerFolder);
                
                repositoryData = ScriptableObject.CreateInstance<ModulesNavigationData>();
                AssetDatabase.CreateAsset(repositoryData, repositoryDataPath);
                AssetDatabase.SaveAssets();
            }
        }
    }
}