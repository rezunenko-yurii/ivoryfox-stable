using System.Collections.Generic;
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
            List<string> toUpdate = new List<string> {url};
            GetAllGitDependencies(toUpdate);
            toUpdate.Reverse();

            InstallUnityPackages();
            UnityRegistryHelper.Download(toUpdate);
            UnityRegistryHelper.Download(unityDependencies);
        }

        public void InstallUnityPackages()
        {
            UnityPackagesInstaller.Install(unityPackages);
        }

        public void Remove()
        {
            UnityRegistryHelper.Remove(new List<string>(){packageId});
        }
        public List<string> GetAllGitDependencies(List<string> toUpdate)
        {
            if (gitDependencies != null)
            {
                foreach (Package package in gitDependencies)
                {
                    if (!toUpdate.Contains(package.url))
                    {
                        toUpdate.Add(package.url);
                    
                        var n = package.GetAllGitDependencies(toUpdate);
                        toUpdate = toUpdate.Concat(n).ToList();
                    }
                } 
            }

            return toUpdate;
        }
    }
}