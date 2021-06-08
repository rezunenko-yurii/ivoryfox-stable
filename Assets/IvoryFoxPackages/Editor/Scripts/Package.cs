using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IvoryFoxPackages.Editor.Scripts.UnityPackages;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackageModel", menuName = "IvoryFox/Create/PackageModel", order = 0)]
    public class Package : ScriptableObject
    {
        public event Action OnGitPackagesJsonLoaded;
        
        [System.NonSerialized]
        public PackageModel localPackage;
        
        [System.NonSerialized]
        public PackageModel gitPackage;
        
        public string packageName;
        public string packageId;
        public string pathToPlugin;
        
        public List<Package> gitDependencies = new List<Package>();
        public List<UnityPackageData> unityPackages = new List<UnityPackageData>();

        public string GetUrl => $"https://github.com/rezunenko-yurii/ivoryfox-stable.git?path={pathToPlugin}";
        
        public void InstallOrUpdate()
        {
            Queue<Package> toUpdate = new Queue<Package>();
            toUpdate = GetAllGitDependencies(toUpdate);
            toUpdate.Reverse();

            Debug.Log("-------------- Packages to install:");
            foreach (Package s in toUpdate)
            {
                Debug.Log(s.packageName);
            }
            Debug.Log("-------------- ");
            
            //PackageManager.OnPackagesFiltrated += UnityRegistryHelper.Download;

            EditorCoroutineUtility.StartCoroutineOwnerless(FiltratePackages(toUpdate));
        }

        public IEnumerator FiltratePackages(Queue<Package> packages)
        {
            Debug.Log($"PackageManager FiltratePackages packages {packages.Count}");
            var installedPackages = UnityRegistryHelper.GetInstalledPackages();
            Queue<Package> packagesForRemove = new Queue<Package>();

            foreach (Package package in packages)
            {
                package.LoadLocalPackageJson();
                
                var installedPackage = installedPackages.FirstOrDefault(x =>
                {
                    string s = $"{package.packageId}@{package.GetUrl}";
                    Debug.Log($"---compare {x.packageId} & {s}");
                    
                    return x.packageId.Equals(s);
                });

                if (installedPackage != null)
                {
                    Debug.Log($"PackageManager found installed packag {installedPackage.name}");
                    
                    yield return EditorCoroutineUtility.StartCoroutineOwnerless(PackageManager.GetPackageFromGit(package,answer =>
                    {
                        package.gitPackage = JsonUtility.FromJson<PackageModel>(answer);
                        Debug.Log($"PackageManager git package loaded");
                    }));
                    
                    Version installedVersion = new Version(installedPackage.version);
                    Version gitVersion = new Version(package.gitPackage.version);

                    Debug.Log($"PackageManager gitVersion = {gitVersion} // installedVersion = {installedVersion}");
                    
                    if (gitVersion <= installedVersion)
                    {
                        Debug.Log($"PackageManager version is lower // remove");
                        packagesForRemove.Enqueue(package);
                    }
                }
            }
            
            var toInstall = new Queue<Package>(packages.Union(packagesForRemove));
            
            Debug.Log($"PackageManager initial count = {packages.Count} // left = {toInstall.Count}");

            UnityRegistryHelper.OnAddRequestComplete += InstallUnityPackages;
            UnityRegistryHelper.Download(toInstall);
        }

        public void InstallUnityPackages()
        {
            Debug.Log($"InstallUnityPackages // count {unityPackages.Count}");
            if (unityPackages.Count > 0)
            {
                var packageInfo = UnityRegistryHelper.GetInstalledPackage(packageId);
                
                if (packageInfo != null) UnityPackagesInstaller.Install(unityPackages, packageInfo.resolvedPath);
                else Debug.Log($"Can`t find installed package {packageName}");
            }
        }

        public void Remove()
        {
            var removeQueue = new Queue<Package>();
            removeQueue.Enqueue(this);
            
            UnityRegistryHelper.Remove(removeQueue);
        }
        private Queue<Package> GetAllGitDependencies(Queue<Package> toUpdate)
        {
            //Debug.Log($"In GetAllGitDependencies of {packageName}");
            
            if (!toUpdate.Contains(this))
            {
                //Debug.Log($"adding to list {url}");
                toUpdate.Enqueue(this);
            }
            else
            {
                //Debug.Log($"already in list {url}");
            }
                
            if (gitDependencies != null)
            {
                foreach (var package in gitDependencies)
                {
                    if (!toUpdate.Contains(package))
                    {
                        //Debug.Log($"adding to list {package.packageName}");
                        toUpdate.Enqueue(package);
                    
                        var n = package.GetAllGitDependencies(toUpdate);
                        toUpdate = new Queue<Package>(toUpdate.Union(n));
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

        public void PreparePackages()
        {
            LoadLocalPackageJson();
            LoadGitPackageJson();
        }
        public void LoadLocalPackageJson()
        {
            TextAsset packageInPackages = (TextAsset) AssetDatabase.LoadAssetAtPath($"Packages/{packageId}/package.json", typeof(TextAsset));
            TextAsset packageInAssets = (TextAsset)AssetDatabase.LoadAssetAtPath($"{pathToPlugin}/package.json", typeof(TextAsset));

            if (packageInPackages != null)
            {
                localPackage = JsonUtility.FromJson<PackageModel>(packageInPackages.text);
            }
            else if (packageInAssets != null)
            {
                localPackage = JsonUtility.FromJson<PackageModel>(packageInAssets.text);
            }
            else
            {
                Debug.Log($"---------- Can`t find {packageId} in the project");
            }
        }

        public void LoadGitPackageJson()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(PackageManager.GetPackageFromGit(this,answer =>
            {
                gitPackage = JsonUtility.FromJson<PackageModel>(answer);
                OnGitPackagesJsonLoaded?.Invoke();
            }));
        }
        
    }
}
