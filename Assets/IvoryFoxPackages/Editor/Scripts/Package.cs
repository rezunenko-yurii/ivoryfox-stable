using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using IvoryFoxPackages.Editor.Scripts.UnityPackages;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackageModel", menuName = "IvoryFox/Create/PackageModel", order = 0)]
    public class Package : ScriptableObject
    {
        public string packageName;
        public string packageId;
        public PackageTypes type;
        public string url;
        public List<string> unityDependencies = new List<string>();
        public List<Package> gitDependencies = new List<Package>();
        
        public List<UnityPackageData> unityPackages = new List<UnityPackageData>();

        public void InstallOrUpdate()
        {
            Queue<string> toUpdate = new Queue<string>();
            toUpdate = GetAllGitDependencies(toUpdate);
            toUpdate.Reverse();

            Debug.Log("-------------- Packages to install:");
            foreach (string s in toUpdate)
            {
                Debug.Log(s);
            }
            Debug.Log("-------------- ");

            //yield return EditorCoroutineUtility.StartCoroutine(UnityRegistryHelper.Download(toUpdate), this);
            UnityRegistryHelper.OnAddRequestComplete += InstallUnityPackages;
            UnityRegistryHelper.Download(toUpdate);
        }

        public void InstallUnityPackages()
        {
            Debug.Log($"InstallUnityPackages // count {unityPackages.Count}");
            if (unityPackages.Count > 0)
            {
                var packageInfo = UnityRegistryHelper.GetInstalledPackage(packageName);

                /*var count = unityPackages.Count;
                for (var i = 0; i < count; i++)
                {
                    var unityPackage = unityPackages[i];
                    if (AssetDatabase.IsValidFolder(unityPackage.pathToPackage))
                    {
                        Debug.Log($"Can`t instal package {unityPackage.packageName} // Is already exist in project");
                        unityPackages.Remove(unityPackages[i]);
                        continue;
                    }
                    else
                    {
                        //Debug.Log($"Can`t instal package {unityPackage.packageName} // Is invalid folder path");
                    }
                    
                }*/

                if (packageInfo != null) UnityPackagesInstaller.Install(unityPackages, packageInfo.resolvedPath);
                else Debug.Log($"Can`t find installed package {packageName}");
            }
        }

        public void Remove()
        {
            var removeQueue = new Queue<string>();
            removeQueue.Enqueue(packageId);
            
            UnityRegistryHelper.Remove(removeQueue);
        }
        private Queue<string> GetAllGitDependencies(Queue<string> toUpdate)
        {
            //Debug.Log($"In GetAllGitDependencies of {packageName}");
            
            if (!toUpdate.Contains(url))
            {
                //Debug.Log($"adding to list {url}");
                toUpdate.Enqueue(url);
            }
            else
            {
                //Debug.Log($"already in list {url}");
            }
                
            if (gitDependencies != null)
            {
                foreach (var package in gitDependencies)
                {
                    if (!toUpdate.Contains(package.url))
                    {
                        //Debug.Log($"adding to list {package.packageName}");
                        toUpdate.Enqueue(package.url);
                    
                        var n = package.GetAllGitDependencies(toUpdate);
                        toUpdate = new Queue<string>(toUpdate.Union(n));
                        //toUpdate = toUpdate.Union(n).ToList();
                    }
                    else
                    {
                        //Debug.Log($"already in list {package.packageName}");
                    }
                } 
            }

            return toUpdate;
        }
    }
}
