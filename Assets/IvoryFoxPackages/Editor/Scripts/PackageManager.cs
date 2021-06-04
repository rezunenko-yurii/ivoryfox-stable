using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class PackageManager
    {
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
    }
}