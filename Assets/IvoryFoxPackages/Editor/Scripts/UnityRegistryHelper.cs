using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class UnityRegistryHelper
    {
        static AddRequest _addRequest;
        static Queue<string> packagesQueue;
 
// this is called via a UI button
        static public void Download(Queue<string> packageList)
        {
            packagesQueue = packageList;
            
            EditorApplication.update += AddProgress;
            EditorApplication.LockReloadAssemblies();
 
            var nextRequestStr = packageList.Dequeue();
            _addRequest = Client.Add(nextRequestStr);
        }
        
        static RemoveRequest _removeRequest;
        static public void Remove(Queue<string> packageList)
        {
            packagesQueue = packageList;
            
            EditorApplication.update += AddProgress;
            EditorApplication.LockReloadAssemblies();
 
            var nextRequestStr = packagesQueue.Dequeue();
            _removeRequest = Client.Remove(nextRequestStr);
        }
        
        static void AddProgress() 
        {
            if (_addRequest.IsCompleted) 
            {
                switch (_addRequest.Status) 
                {
                    case StatusCode.Failure:    // couldn't remove package
                        Debug.LogError("Couldn't Download package '" + _addRequest.Result.name + "': " + _addRequest.Error.message);
                        break;
 
                    case StatusCode.InProgress:
                        break;
 
                    case StatusCode.Success:
                        Debug.Log("Download package: " + _addRequest.Result.name);
                        break;
                }
 
                if (packagesQueue.Count > 0) 
                {
                    var nextRequestStr = packagesQueue.Dequeue();
                    Debug.Log("Requesting removal of '" + nextRequestStr + "'.");
                    _addRequest = Client.Add(nextRequestStr);
 
                } else {    // no more packages to remove
                    EditorApplication.update -= AddProgress;
                    EditorApplication.UnlockReloadAssemblies();
                }
            }
        }
        
        static void RemoveProgress() 
        {
            if (_removeRequest.IsCompleted) 
            {
                switch (_removeRequest.Status) 
                {
                    case StatusCode.Failure:    // couldn't remove package
                        Debug.LogError("Couldn't Download package '" + _removeRequest.PackageIdOrName + "': " + _addRequest.Error.message);
                        break;
 
                    case StatusCode.InProgress:
                        break;
 
                    case StatusCode.Success:
                        Debug.Log("Download package: " + _removeRequest.PackageIdOrName);
                        break;
                }
 
                if (packagesQueue.Count > 0) 
                {
                    var nextRequestStr = packagesQueue.Dequeue();
                    Debug.Log("Requesting removal of '" + nextRequestStr + "'.");
                    _removeRequest = Client.Remove(nextRequestStr);
 
                } else {    // no more packages to remove
                    EditorApplication.update -= RemoveProgress;
                    EditorApplication.UnlockReloadAssemblies();
                }
            }
        }
        
        /*public static IEnumerator Download(List<string> packageList)
        {
            Debug.Log($"In UnityRegistryHelper Download // {packageList.Count} packages to download");
            
            foreach (string package in packageList)
            {
                if (!string.IsNullOrEmpty(package))
                {
                    Debug.Log($"Start to download {package}");
                    
                    AddRequest request = Client.Add(package);
 
                    while (request.Status == StatusCode.InProgress)
                    {
                        yield return null;
                    }
 
                    if (request.Status == StatusCode.Success)
                    {
                        Debug.Log($"{package} successfully downloaded");
                    }
                    else
                    {
                        Debug.LogError("Cannot import " + package + ": " + request.Error.message);
                    }

                    if (!request.IsCompleted)
                    {
                        Debug.Log($"request still isn`t complete // waiting");
                        
                        while (!request.IsCompleted)
                        {
                            yield return null;
                        }
                        
                        Debug.Log($"request is complete");
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
        }*/
        
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