using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    public class UnityUrlLoader: MonoBehaviour, IUrlLoader
    {
        public event Action<string> OnFailure;
        public event Action<string> OnSuccess;
        public IMediator Mediator { get; private set; }
        public string ConfigName { get; } = "urlsConfig";

        private Coroutine _coroutine;
        private UrlsConfig _urlsConfig;
        
        public void SetConfig(string json) => _urlsConfig = JsonUtility.FromJson<UrlsConfig>(json);
        public string GetUrl() => _urlsConfig.url;

        public void SetMediator(IMediator mediator)
        {
            Mediator = mediator;
        }
        public void DoRequest()
        {
            Debug.Log("UnityUrlLoader DoRequest");
            
            if (_urlsConfig == null)
            {
                OnFailure?.Invoke("urlsConfig is null");
                Mediator.Notify(this,"Error");
                
                return;
            }

            if (_urlsConfig.HasUrl)
            {
                OnSuccess?.Invoke(_urlsConfig.url);
                Mediator.Notify(this,"OnUrlLoaded");
            }
            else if (_urlsConfig.HasServer)
            {
                _coroutine = StartCoroutine(SendGet());
            }
            else
            {
                OnFailure?.Invoke("urlsConfig either hasn`t nor url nor server");
                Mediator.Notify(this,"Error");
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
                    OnFailure?.Invoke(webRequest.error);
                    Mediator.Notify(this,"Error");
                    
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
                OnFailure?.Invoke("UnityUrlLoader // Can`t read response from UrlLoader");
                Mediator.Notify(this,"Error");
            }
            else
            {
                _urlsConfig.url = url;
                OnSuccess?.Invoke(url);
                Mediator.Notify(this,"OnUrlLoaded");
            }
            
            RemoveListeners();
        }
        
        public void RemoveListeners()
        {
            Debug.Log("UnityUrlLoader RemoveListeners");
            
            OnSuccess = OnFailure = null;
            StopCoroutine(_coroutine);
        }

        public IModulesHost Parent { get; set; }
    }
}