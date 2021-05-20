using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class UnityRegistryHelper
    {
        public static IEnumerator Download(List<string> packageList)
        {
            Debug.Log($"In UnityRegistryHelper Download // {packageList.Count} packages to download");
            
            foreach (string package in packageList)
            {
                if (!string.IsNullOrEmpty(package))
                {
                    Debug.Log($"Start to download {package}");
                    AddRequest request = Client.Add(package);
 
                    while (!request.IsCompleted)
                    {
                        yield return null;
                        //System.Threading.Tasks.Task.Delay(100);
                    }
 
                    if (request.Status != StatusCode.Success)
                    {
                        Debug.LogError("Cannot import " + package + ": " + request.Error.message);
                    }
                    else
                    {
                        Debug.Log($"{package} successfully downloaded");
                    }
                }
            }
            
            Debug.Log($"UnityRegistryHelper Download Complete");
        }
        
        public static void Remove(List<string> packageList)
        {
            foreach (string package in packageList)
            {
                if (!string.IsNullOrEmpty(package))
                {
                    RemoveRequest request = Client.Remove(package);
 
                    while (!request.IsCompleted)
                    {
                        System.Threading.Tasks.Task.Delay(100);
                    }
 
                    if (request.Status != StatusCode.Success)
                    {
                        Debug.LogError("Cannot Remove " + package + ": " + request.Error.message);
                    }
                }
            }
        }
        
        public static PackageCollection GetInstalledPackages()
        {
            Debug.Log($"In UnityRegistryHelper GetInstalledPackages");
            
            ListRequest request = Client.List(false, true);

            while (!request.IsCompleted)
            {
                System.Threading.Tasks.Task.Delay(100);
            }

            return request.Result;
        }
        
        public static PackageInfo GetInstalledPackage(string packageName)
        {
            var a = GetInstalledPackages();
            
            /*foreach (var p in a)
            {
                Debug.Log(p.packageId + " " + p.resolvedPath);
            }*/
            
            var packageInfo = a.SingleOrDefault(x => x.packageId.Contains(packageName));
            
            return packageInfo;
        }
    }
}