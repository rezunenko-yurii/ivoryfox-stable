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
        
        private static string _packageLocation;
        //private static string 
        static UnityPackagesInstaller()
        {
            //packagesPath = Application.dataPath.Replace("/Assets", "");
        }
        public static void Install(List<UnityPackageData> all, string packageLocation)
        {
            Debug.Log($"UnityPackagesInstaller Install {Queue.Count} // package location {packageLocation}");
            
            bool shouldStart = Queue.Count == 0;

            foreach (var toInstall in all)
            {
                if (!Queue.Contains(toInstall))
                {
                    Debug.Log($"UnityPackagesInstaller Added {toInstall} to queue");
                    Queue.Enqueue(toInstall);
                }
            }

            if (shouldStart)
            {
                Debug.Log($"UnityPackagesInstaller Install shouldStart == true");
                _packageLocation = packageLocation;
                
                AddSubscribes();
                StartInstall();
            }
            else
            {
                Debug.Log($"UnityPackagesInstaller Install shouldStart == false");
                
                OnAllInstalled?.Invoke();
                OnAllInstalled = null;
            }
        }

        private static void StartInstall()
        {
            Debug.Log($"UnityPackagesInstaller StartInstall {Queue.Count}");
            
            if (Queue.Count > 0)
            {
                UnityPackageData package = Queue.Dequeue();

                if (!Directory.Exists(package.installedPackageLocation))
                {
                    Debug.Log($"UnityPackagesInstaller Trying to install {package.packageName} {_packageLocation + package.pathToPackage}");
                    AssetDatabase.ImportPackage(_packageLocation + package.pathToPackage, true);
                }
                else Debug.Log($"{package.packageName} is exists in project");
            }
            else
            {
                Debug.Log("UnityPackagesInstaller Queue.Count < 0");
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