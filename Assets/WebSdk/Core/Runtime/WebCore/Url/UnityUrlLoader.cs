using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    public class UnityUrlLoader: MonoBehaviour, IUrlLoader
    {
        public event Action<string> LoadingFailed;
        public event Action<string> LoadingSucceeded;
        public string ConfigName { get; } = "urlsConfig";

        private Coroutine _coroutine;
        private UrlsConfig _urlsConfig;
        
        public void SetConfig(string json) => _urlsConfig = JsonUtility.FromJson<UrlsConfig>(json);
        public string GetUrl() => _urlsConfig.url;
        
        public void DoRequest()
        {
            Debug.Log("UnityUrlLoader DoRequest");
            
            if (_urlsConfig == null)
            {
                LoadingFailed?.Invoke("urlsConfig is null");
                return;
            }

            if (_urlsConfig.HasUrl)
            {
                LoadingSucceeded?.Invoke(_urlsConfig.url);
            }
            else if (_urlsConfig.HasServer)
            {
                _coroutine = StartCoroutine(SendGet());
            }
            else
            {
                LoadingFailed?.Invoke("urlsConfig either hasn`t nor url nor server");
            }
        }
        
        private IEnumerator SendGet()
        {
            Debug.Log("UnityUrlLoader SendGet");
            
            using (UnityWebRequest webRequest  = UnityWebRequest.Get(_urlsConfig.server))
            {
                webRequest.timeout = 12;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    LoadingFailed?.Invoke(webRequest.error);

                    yield break;
                }
                
                OnGetResponse(webRequest.downloadHandler.text);
            }
        }
        public void OnGetResponse(string response)
        {
            Debug.Log("UnityUrlLoader OnGetResponse");
            
            UrlResponseReader reader = new UrlResponseReader();
            string url = reader.GetUrl(response);

            if (string.IsNullOrEmpty(url))
            {
                LoadingFailed?.Invoke("UnityUrlLoader // Can`t read response from UrlLoader");
            }
            else
            {
                _urlsConfig.url = url;
                LoadingSucceeded?.Invoke(url);
            }
            
            RemoveListeners();
        }
        
        public void RemoveListeners()
        {
            Debug.Log("UnityUrlLoader RemoveListeners");
            
            LoadingSucceeded = LoadingFailed = null;
            StopCoroutine(_coroutine);
        }
    }
}