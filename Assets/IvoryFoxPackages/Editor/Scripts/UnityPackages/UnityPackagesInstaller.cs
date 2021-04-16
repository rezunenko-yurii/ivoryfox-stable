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
                
                if (!Directory.Exists(package.installedPackageLocation)) AssetDatabase.ImportPackage(package.pathToPackage, true);
                else Debug.Log($"{package.packageName} is exists in project");
            }
            else
            {
                RemoveSubscribes();
                
                OnAllInstalled?.Invoke();
                OnAllInstalled = null;
            }
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