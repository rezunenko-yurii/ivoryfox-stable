using System.Collections.Generic;
using System.IO;
using System.Linq;
using IvoryFoxPackages.Editor.Scripts.UnityPackages;
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
            //UnityPackagesInstaller.OnAllInstalled += UnityPackagesInstallerOnOnAllInstalled;
            //InstallUnityPackages();
            
            //List<string> toUpdate = new List<string> {url};
            List<string> toUpdate = new List<string>();
            GetAllGitDependencies(toUpdate);
            toUpdate.Reverse();

            Debug.Log("-------------- Packages to install:");
            foreach (string s in toUpdate)
            {
                Debug.Log(s);
            }
            Debug.Log("-------------- ");
            
            UnityRegistryHelper.Download(toUpdate);
            InstallUnityPackages();
        }

        public void InstallUnityPackages()
        {
            if (unityPackages.Count > 0)
            {
                var packageInfo = UnityRegistryHelper.GetInstalledPackage(packageName);
                
                if (packageInfo != null) UnityPackagesInstaller.Install(unityPackages, packageInfo.resolvedPath);
                else Debug.Log($"Ca`nt find installed package {packageName}");
            }
        }

        private void UnityPackagesInstallerOnOnAllInstalled()
        {
            List<string> toUpdate = new List<string> {url};
            GetAllGitDependencies(toUpdate);
            toUpdate.Reverse();
            
            UnityRegistryHelper.Download(toUpdate);
            //UnityRegistryHelper.Download(unityDependencies);
        }

        public void Remove()
        {
            UnityRegistryHelper.Remove(new List<string>(){packageId});
        }
        public List<string> GetAllGitDependencies(List<string> toUpdate)
        {
            if (!toUpdate.Contains(url))
            {
                Debug.Log($"adding to list {url}");
                toUpdate.Add(url);
            }
            else
            {
                Debug.Log($"already in list {url}");
            }
                
            if (gitDependencies != null)
            {
                foreach (Package package in gitDependencies)
                {
                    if (!toUpdate.Contains(package.url))
                    {
                        Debug.Log($"adding to list {package.packageName}");
                        toUpdate.Add(package.url);
                    
                        var n = package.GetAllGitDependencies(toUpdate);
                        toUpdate = toUpdate.Concat(n).ToList();
                    }
                    else
                    {
                        Debug.Log($"already in list {package.packageName}");
                    }
                } 
            }

            return toUpdate;
        }
    }
}
