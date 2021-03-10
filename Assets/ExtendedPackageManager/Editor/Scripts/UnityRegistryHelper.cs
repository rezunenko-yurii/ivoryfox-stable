using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace ExtendedPackageManager.Editor.Scripts
{
    public static class UnityRegistryHelper
    {
        static ListRequest _listRequest;
        private static List<string> _packages;

        public static void Download(List<string> packages)
        {
            if(packages is null || packages.Count == 0) return;
            _packages = packages;
            
            _listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        static void ListProgress()
        {
            if (_listRequest.IsCompleted)
            {
                if (_listRequest.Status == StatusCode.Success)
                {
                    TryToDownload();
                }
                else if (_listRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(_listRequest.Error.message);
                }

                EditorApplication.update -= ListProgress;
            }
        }

        private static void TryToDownload()
        {
            Debug.Log($"Trying To Download Unity Registry Packages | found files {_packages.Count} to download");

            foreach (var package in _packages)
            {
                Debug.Log($"Checking {package}...");
                    
                if (_listRequest.Result.Any(p => p.name.Equals(package)))
                {
                    Debug.Log($"{package} is already in project");
                }
                else
                {
                    Debug.Log($"Downloading and installing {package}...");
                    Client.Add(package);
                }
            }

            _packages = null;
        }
    }
}