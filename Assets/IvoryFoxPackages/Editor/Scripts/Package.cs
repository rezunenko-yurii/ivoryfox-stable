using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using IvoryFoxPackages.Editor.Scripts.UnityPackages;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace IvoryFoxPackages.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackageModel", menuName = "IvoryFox/Create/PackageModel", order = 0)]
    public class Package : ScriptableObject
    {
        //public TextAsset packageAsset;
        public PackageModel localPackage;
        public PackageModel gitPackage;
        
        public string packageName;
        public string packageId;
        //private string pathToPackageJson;
        public string pathToPlugin;
        
        public PackageTypes type;
        //public string url;
        public List<string> unityDependencies = new List<string>();
        public List<Package> gitDependencies = new List<Package>();

        public string GetUrl => $"https://github.com/rezunenko-yurii/ivoryfox-stable.git?path={pathToPlugin}";
        
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
                var packageInfo = UnityRegistryHelper.GetInstalledPackage(packageId);

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
            
            if (!toUpdate.Contains(GetUrl))
            {
                //Debug.Log($"adding to list {url}");
                toUpdate.Enqueue(GetUrl);
            }
            else
            {
                //Debug.Log($"already in list {url}");
            }
                
            if (gitDependencies != null)
            {
                foreach (var package in gitDependencies)
                {
                    if (!toUpdate.Contains(package.GetUrl))
                    {
                        //Debug.Log($"adding to list {package.packageName}");
                        toUpdate.Enqueue(package.GetUrl);
                    
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

        public void PreparePackages()
        {
            TextAsset packageAsset = (TextAsset)AssetDatabase.LoadAssetAtPath($"Packages/{packageId}/package.json", typeof(TextAsset));
            if (packageAsset is null)
            {
                packageAsset = (TextAsset)AssetDatabase.LoadAssetAtPath($"{pathToPlugin}/package.json", typeof(TextAsset));
            }

            if (packageAsset is null)
            {
                Debug.Log($"---------- Can`t find {packageId} in the project");
            }
            else
            {
                localPackage = JsonUtility.FromJson<PackageModel>(packageAsset.text);
                //pathToPackageJson = AssetDatabase.GetAssetPath(packageAsset);
                
                Debug.Log($"{packageName} Path to local package.json is {packageAsset.text}");
            }
            
            EditorCoroutineUtility.StartCoroutineOwnerless(SendGet());
        }
        
        private IEnumerator SendGet()
        {
            /*if (string.IsNullOrEmpty(pathToPackageJson))
            {
                Debug.Log($"{packageName} Path to git package.json is null");
                yield break;
            }*/

            string url = $"https://raw.githubusercontent.com/rezunenko-yurii/ivoryfox-stable/master/{pathToPlugin}/package.json";
            Debug.Log($"Package: Loading git package {url}");
            
            using (UnityWebRequest webRequest  = UnityWebRequest.Get(url))
            {
                webRequest.timeout = 12;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    yield break;
                }
                
                gitPackage = JsonUtility.FromJson<PackageModel>(webRequest.downloadHandler.text);
                
                Debug.Log($"Package: Loading Complete --{gitPackage.name} {gitPackage.version}");
            }
        }
    }
}
