using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace IvoryFoxPackages.Editor.Scripts
{
    public static class GitFilesReader
    {
        public static async Task GetFile()
        {
            Debug.Log("IN REPO TEST");
            //https://github.com/rezunenko-yurii/ivoryfox-stable.git?path=Assets/AdjustHelper
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApplication", "1"));
            
            var repo = "rezunenko-yurii/ivoryfox-stable";
            
            //https://github.com/rezunenko-yurii/ivoryfox-stable/blob/master/Assets/AdjustHelper/package.json
            //https://api.github.com/repos/rezunenko-yurii/ivoryfox-stable/contents/Assets/AdjustHelper/package.json?ref=master
            //https://raw.githubusercontent.com/rezunenko-yurii/ivoryfox-stable/master/Assets/AdjustHelper/package.json
            
            var contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            var contentsJson = await httpClient.GetStringAsync("https://raw.githubusercontent.com/rezunenko-yurii/ivoryfox-stable/master/Assets/AdjustHelper/package.json");
            //var contentsJson = await httpClient.GetStringAsync(contentsUrl);
            var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);
            
            Debug.Log("IN REPO TEST 2");
            
            foreach(var file in contents)
            {
                var fileType = (string)file["type"];
                if (fileType == "dir")
                {
                    var directoryContentsUrl = (string)file["url"];
                    // use this URL to list the contents of the folder
                    Debug.Log($"DIR: {directoryContentsUrl}");
                }
                else if (fileType == "file")
                {
                    var downloadUrl = (string)file["download_url"];
                    // use this URL to download the contents of the file
                    Debug.Log($"DOWNLOAD: {downloadUrl}");
                }
            }
        }
        
        [MenuItem("IvoryFox/REPO TEST")]
        public static void Test()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(SendGet());
        }
        
        private static IEnumerator SendGet()
        {
            using (UnityWebRequest webRequest  = UnityWebRequest.Get("https://raw.githubusercontent.com/rezunenko-yurii/ivoryfox-stable/master/Assets/AdjustHelper/package.json"))
            {
                webRequest.timeout = 12;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    //OnFailure?.Invoke(webRequest.error);

                    yield break;
                }
                Debug.Log(webRequest.downloadHandler.text);
                //OnGetResponse(webRequest.downloadHandler.text);
            }
        }
    }
}