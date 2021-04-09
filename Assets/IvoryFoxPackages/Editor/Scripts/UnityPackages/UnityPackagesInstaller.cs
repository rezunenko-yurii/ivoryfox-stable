using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts.UnityPackages
{
    public static class UnityPackagesInstaller
    {
        private static readonly Queue<UnityPackageData> Queue = new Queue<UnityPackageData>();
        public static void Install(List<UnityPackageData> all)
        {
            bool shouldStart = Queue.Count == 0;

            foreach (var toInstall in all)
            {
                if (!Queue.Contains(toInstall)) Queue.Enqueue(toInstall);
            }

            if (shouldStart)
            {
                AddSubscribes();
                StartInstall();
            }
        }

        private static void StartInstall()
        {
            if (Queue.Count > 0)
            {
                UnityPackageData package = Queue.Dequeue();
                
                if (!Directory.Exists(package.installedPackageLocation)) AssetDatabase.ImportPackage(package.pathToPackage, true);
                else Debug.Log($"{package.packageName} is exists in project");
            }
            else RemoveSubscribes();
        }
        
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
        
        private static void AddSubscribes()
        {
            AssetDatabase.importPackageCompleted += ImportStarted;
            AssetDatabase.importPackageFailed += ImportFailed;
            AssetDatabase.importPackageCancelled += ImportCancelled;
        }

        private static void RemoveSubscribes()
        {
            AssetDatabase.importPackageCompleted -= ImportStarted;
            AssetDatabase.importPackageFailed -= ImportFailed;
            AssetDatabase.importPackageCancelled -= ImportCancelled;
        }

        private static void ImportStarted(string packageName)
        {
            Debug.Log($"Import started {packageName}");
        }
    }
}