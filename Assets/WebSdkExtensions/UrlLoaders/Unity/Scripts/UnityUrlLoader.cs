using System;
using System.Collections;
using GlobalBlock.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace WebSdkExtensions.UrlLoaders.Unity.Scripts
{
    public class UnityUrlLoader: IUrlLoader
    {
        public event Action<string> OnFailure;
        public event Action<string> OnSuccess;
        
        public IMediator mediator { get; private set; }
        public string ConfigName { get; } = "urlsConfig";

        private Coroutine coroutine;
        private UrlsConfig urlsConfig;
        
        
        public void SetConfig(string json) => urlsConfig = JsonUtility.FromJson<UrlsConfig>(json);
        public string GetUrl() => urlsConfig.url;

        public void SetMediator(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public void Start()
        {
            if (urlsConfig == null)
            {
                OnFailure?.Invoke("urlsConfig is null");
                mediator.Notify(this,"Error");
                
                return;
            }

            if (urlsConfig.HasUrl)
            {
                OnSuccess?.Invoke(urlsConfig.url);
                mediator.Notify(this,"OnUrlLoaded");
            }
            else if (urlsConfig.HasServer)
            {
                coroutine = GlobalFacade.monoBehaviour.StartCoroutine(SendGet());
            }
            else
            {
                OnFailure?.Invoke("urlsConfig either hasn`t nor url nor server");
                mediator.Notify(this,"Error");
            }
        }
        
        private IEnumerator SendGet()
        {
            using (UnityWebRequest webRequest  = UnityWebRequest.Get(urlsConfig.server))
            {
                webRequest.timeout = 12;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    OnFailure?.Invoke(webRequest.error);
                    mediator.Notify(this,"Error");
                    
                    yield break;
                }
                
                OnGetResponse(webRequest.downloadHandler.text);
            }
        }
        public void OnGetResponse(string response)
        {
            UrlResponseReader reader = new UrlResponseReader();
            string url = reader.GetUrl(response);

            if (string.IsNullOrEmpty(url))
            {
                OnFailure?.Invoke("Can`t read response from UrlLoader");
                mediator.Notify(this,"Error");
            }
            else
            {
                urlsConfig.url = url;
                OnSuccess?.Invoke(url);
                mediator.Notify(this,"OnUrlLoaded");
            }
            
            RemoveListeners();
        }
        
        public void RemoveListeners()
        {
            OnSuccess = OnFailure = null;
            GlobalFacade.monoBehaviour.StopCoroutine(coroutine);
        }
    }
}