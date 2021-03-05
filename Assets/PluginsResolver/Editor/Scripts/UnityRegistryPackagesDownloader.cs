using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace PluginsResolver.Editor.Scripts
{
    public static class UnityRegistryPackagesDownloader
    {
        static ListRequest listRequest;

        public static void Download()
        {
            listRequest = Client.List();
            EditorApplication.update += ListProgress;
        }

        static void ListProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    TryToDownload();
                }
                else if (listRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(listRequest.Error.message);
                }

                EditorApplication.update -= ListProgress;
            }
        }

        private static void TryToDownload()
        {
            var all = Resources.LoadAll<PackagesToDownload>("");
            Debug.Log($"Trying To Download Unity Registry Packages | found files {all.Length} to download");

            foreach (var packagesToDownload in all)
            {
                Debug.Log($"{packagesToDownload.name} has {packagesToDownload.packages.Count} dependencies to download");
                
                foreach (var package in packagesToDownload.packages)
                {
                    Debug.Log($"Checking {package}...");
                    
                    if (listRequest.Result.Any(p => p.name.Equals(package)))
                    {
                        Debug.Log($"{package} is already in project");
                    }
                    else
                    {
                        Debug.Log($"Downloading and installing {package}...");
                        Client.Add(package);
                    }
                }
            }
        }
    }
}