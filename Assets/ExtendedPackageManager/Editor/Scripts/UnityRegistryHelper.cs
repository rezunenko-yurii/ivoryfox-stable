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
        static AddRequest _addRequest;
        private static List<string> _packages;

        public static void Download(List<string> packages)
        {
            if(packages is null || packages.Count == 0) return;
            _packages = packages;

            Debug.Log("Next packages will be installed/updated:");
            foreach (var package in packages)
            {
                Debug.Log(package);
            }
            
            TryToDownload();
            
            /*_listRequest = Client.List();
            EditorApplication.update += ListProgress;*/
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
            //Debug.Log($"Trying To Download Unity Registry Packages | found files {_packages.Count} to download");

            if (_packages.Count > 0)
            {
                string f = _packages.First();
                _packages.RemoveAt(0);
                
                Debug.Log($"---------------------");
                Debug.Log($"Downloading and installing {f}...");
                _addRequest = Client.Add(f);
                EditorApplication.update += AddProgress;
            }

            /*foreach (var package in _packages)
            {
                //Debug.Log($"Checking {package}...");

                if (_listRequest.Result.Any(p => p.name.Equals(package)))
                {
                    Debug.Log($"{package} is already in project");
                }
                else
                {
                    Debug.Log($"Downloading and installing {package}...");
                    Client.Add(package);
                }
            }*/

            //_packages = null;
        }

        private static void AddProgress()
        {
            if (_addRequest.IsCompleted)
            {
                EditorApplication.update -= AddProgress;
                
                if (_addRequest.Status == StatusCode.Success)
                {
                    TryToDownload();
                }
                else if (_addRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(_addRequest.Error.message);
                }
            }
        }
    }
}