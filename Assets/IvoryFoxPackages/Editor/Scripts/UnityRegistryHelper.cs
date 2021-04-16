﻿using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class UnityRegistryHelper
    {
        //public static event Action OnInstallComplete;
        public static void Download(List<string> packageList)
        {
            foreach (string package in packageList)
            {
                if (!string.IsNullOrEmpty(package))
                {
                    AddRequest request = Client.Add(package);
 
                    while (!request.IsCompleted)
                    {
                        System.Threading.Tasks.Task.Delay(100);
                    }
 
                    if (request.Status != StatusCode.Success)
                    {
                        Debug.LogError("Cannot import " + package + ": " + request.Error.message);
                    }
                }
            }
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
            ListRequest request = Client.List();
 
            while (!request.IsCompleted)
            {
                System.Threading.Tasks.Task.Delay(100);
            }

            return request.Result;
        }
        
        public static PackageInfo GetInstalledPackage(string packageId)
        {
            SearchRequest request = Client.Search(packageId);
 
            while (!request.IsCompleted)
            {
                System.Threading.Tasks.Task.Delay(100);
            }

            return request.Result[0];
        }
    }
}