using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts.UnityPackages
{
    public static class UnityPackagesInstaller
    {
        public static event Action OnAllInstalled;
        private static readonly Queue<UnityPackageData> Queue = new Queue<UnityPackageData>();
        private static string PackageLocation;
        static UnityPackagesInstaller()
        {
            //packagesPath = Application.dataPath.Replace("/Assets", "");
        }
        public static void Install(List<UnityPackageData> all, string packageLocation)
        {
            bool shouldStart = Queue.Count == 0;

            foreach (var toInstall in all)
            {
                if (!Queue.Contains(toInstall)) Queue.Enqueue(toInstall);
            }

            if (shouldStart)
            {
                PackageLocation = packageLocation;
                
                AddSubscribes();
                StartInstall();
            }
            else
            {
                OnAllInstalled?.Invoke();
                OnAllInstalled = null;
            }
        }

        private static void StartInstall()
        {
            if (Queue.Count > 0)
            {
                UnityPackageData package = Queue.Dequeue();

                if (!Directory.Exists(package.installedPackageLocation))
                {
                    string packagePath = Path.GetFullPath("Packages/com.unity.textmeshpro");
                    
                    Debug.Log($"Trying to install {package.packageName} {PackageLocation + package.pathToPackage}");
                    AssetDatabase.ImportPackage(PackageLocation + package.pathToPackage, true);
                }
                else Debug.Log($"{package.packageName} is exists in project");
            }
            else
            {
                RemoveSubscribes();
                
                OnAllInstalled?.Invoke();
                OnAllInstalled = null;
            }
        }
        
        /*private static string GetPackageFullPath()
        {
            // Check for potential UPM package
            string packagePath = Path.GetFullPath("Packages/com.unity.textmeshpro");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                // Search default location for development package
                if (Directory.Exists(packagePath + "/Assets/Packages/com.unity.TextMeshPro/Editor Resources"))
                {
                    return packagePath + "/Assets/Packages/com.unity.TextMeshPro";
                }

                // Search for default location of normal TextMesh Pro AssetStore package
                if (Directory.Exists(packagePath + "/Assets/TextMesh Pro/Editor Resources"))
                {
                    return packagePath + "/Assets/TextMesh Pro";
                }

                // Search for potential alternative locations in the user project
                string[] matchingPaths = Directory.GetDirectories(packagePath, "TextMesh Pro", SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return packagePath + path;
            }

            return null;
        }*/
        
        private static void ImportCancelled(string packageName)
        {
            Debug.Log($"Import cancelled {packageName}");
            StartInstall();
        }

        private static void ImportFailed(string packageName, string errormessage)
        {
            Debug.Log($"Import {packageName} failed, reason: {errormessage} ");
            StartInstall();
        }
        
        private static void ImportStarted(string packageName)
        {
            Debug.Log($"Import started {packageName}");
        }
        
        private static void ImportCompleted(string packageName)
        {
            Debug.Log($"Import completed {packageName}");
            StartInstall();
        }
        
        private static void AddSubscribes()
        {
            AssetDatabase.importPackageStarted += ImportStarted;
            AssetDatabase.importPackageCompleted += ImportCompleted;
            AssetDatabase.importPackageFailed += ImportFailed;
            AssetDatabase.importPackageCancelled += ImportCancelled;
        }

        private static void RemoveSubscribes()
        {
            AssetDatabase.importPackageStarted -= ImportStarted;
            AssetDatabase.importPackageCompleted -= ImportCompleted;
            AssetDatabase.importPackageFailed -= ImportFailed;
            AssetDatabase.importPackageCancelled -= ImportCancelled;
        }
    }
}