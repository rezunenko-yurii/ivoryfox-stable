using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CloackaV4.Scripts
{
    public class UnityUrlLoader: MonoBehaviour
    {
        public event Action<string> OnFailure;
        public event Action<string> OnSuccess;

        private Coroutine coroutine;

        public void Load(string url, Dictionary<string,string> headers)
        {
            coroutine = StartCoroutine(SendGet(url, headers));
        }
    
        private IEnumerator SendGet(string url, Dictionary<string,string> headers)
        {
            Debug.Log($"url for get-request: {url}");
        
            using UnityWebRequest webRequest  = UnityWebRequest.Get(url);
        
            Debug.Log($"Adding headers --------------------");
            foreach (var header in headers)
            {
                Debug.Log($"Adding header {header.Key} {header.Value}");
                webRequest.SetRequestHeader(header.Key, header.Value);
            }
            Debug.Log($"End of adding headers --------------------");
            
            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.disposeUploadHandlerOnDispose = true;
                
            yield return webRequest.SendWebRequest();

            if (!string.IsNullOrEmpty(webRequest.error))
            {
                OnFailure?.Invoke(webRequest.error);
                yield break;
            }
                
            OnGetResponse(webRequest.downloadHandler.text);
        }

        private void OnGetResponse(string response)
        {
            Debug.Log($"UnityUrlLoader OnGetResponse {response}");
            
            UrlResponseReader reader = new UrlResponseReader();
            string url = reader.GetUrl(response);

            if (string.IsNullOrEmpty(url))
            {
                OnFailure?.Invoke("Can`t read response from UrlLoader");
            }
            else
            {
                OnSuccess?.Invoke(url);
            }
        }
    }
}