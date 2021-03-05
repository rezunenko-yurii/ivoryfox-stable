using System.IO;
using UnityEditor;
using UnityEngine;

namespace PluginsResolver.Editor.Scripts
{
    public static class LocalPackagesInstaller
    {
        public static void TryToInstall()
        {
            Debug.Log($"{nameof(UnityPackagesLoader)}: CheckPackagesToInstall");
            var all = Resources.LoadAll<PackagesToInstall>("");
            foreach (var packagesToInstall in all)
            {
                foreach (LocalPackageData package in packagesToInstall.packages)
                {
                    if (!Directory.Exists(package.installedPackageLocation))
                    {
                        AssetDatabase.importPackageCompleted += ImportStarted;
                        AssetDatabase.importPackageFailed += ImportFailed;
                        AssetDatabase.importPackageCancelled += ImportCancelled;
                        AssetDatabase.ImportPackage(package.pathToPackage, true);
                    }
                    else
                    {
                        Debug.Log($"{package.packageName} is exists in project");
                    }
                }
            }
        }
        
        private static void ImportCancelled(string packagename)
        {
            Debug.Log($"Import cancelled {packagename}");
            RemoveSubscritions();
        }

        private static void ImportFailed(string packagename, string errormessage)
        {
            Debug.Log($"Import {packagename} failed, reason: {errormessage} ");
            RemoveSubscritions();
        }

        private static void RemoveSubscritions()
        {
            AssetDatabase.importPackageCompleted -= ImportStarted;
            AssetDatabase.importPackageFailed -= ImportFailed;
            AssetDatabase.importPackageCancelled -= ImportCancelled;
        }

        private static void ImportStarted(string packagename)
        {
            Debug.Log($"Import started {packagename}");
        }
    }
}