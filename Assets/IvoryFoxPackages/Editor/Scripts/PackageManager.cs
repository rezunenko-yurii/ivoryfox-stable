using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class PackageManager
    {
        public static event Action<Queue<Package>> OnPackagesFiltrated;
        public static IEnumerator GetPackageFromGit(Package package,Action<string> callback)
        {
            string url = $"https://raw.githubusercontent.com/rezunenko-yurii/ivoryfox-stable/master/{package.pathToPlugin}/package.json";
            Debug.Log($"Package: Loading package from git {url}");
            
            using (UnityWebRequest webRequest  = UnityWebRequest.Get(url))
            {
                webRequest.timeout = 12;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();

                if (string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.Log($"Package: Loading Complete --{package.packageId}");
                    callback?.Invoke(webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.Log($"Package: Loading Error --{package.packageId} {webRequest.downloadHandler.error}");
                    yield break;
                }
            }
        }

        /*public static IEnumerator FiltratePackages(Queue<Package> packages)
        {
            Debug.Log($"PackageManager FiltratePackages packages {packages.Count}");
            var installedPackages = UnityRegistryHelper.GetInstalledPackages();
            Queue<Package> packagesForRemove = new Queue<Package>();

            foreach (Package package in packages)
            {
                package.LoadLocalPackageJson();
                
                var installedPackage = installedPackages.First(x => x.packageId == package.packageId);

                if (installedPackage != null)
                {
                    Debug.Log($"PackageManager found installed packag {installedPackage.name}");
                    
                    yield return EditorCoroutineUtility.StartCoroutineOwnerless(GetPackageFromGit(package,answer =>
                    {
                        package.gitPackage = JsonUtility.FromJson<PackageModel>(answer);
                        Debug.Log($"PackageManager git package loaded");
                    }));
                    
                    Version installedVersion = new Version(installedPackage.version);
                    Version gitVersion = new Version(package.gitPackage.version);

                    Debug.Log($"PackageManager gitVersion = {gitVersion} // installedVersion = {installedVersion}");
                    
                    if (gitVersion < installedVersion)
                    {
                        Debug.Log($"PackageManager version is lower // remove");
                        packagesForRemove.Enqueue(package);
                    }
                }
            }
            
            var toInstall = new Queue<Package>(packages.Union(packagesForRemove));
            
            Debug.Log($"PackageManager initial count = {packages.Count} // left = {toInstall.Count}");
            OnPackagesFiltrated?.Invoke(toInstall);
            OnPackagesFiltrated = null;
        }*/
    }
}