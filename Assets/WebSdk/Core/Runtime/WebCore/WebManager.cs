using System;
using System.Diagnostics;

using UnityEngine;

using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore.Parameters;
using WebSdk.Core.Runtime.WebCore.Url;
using WebSdk.Core.Runtime.WebCore.WebView;
using Debug = UnityEngine.Debug;

namespace WebSdk.Core.Runtime.WebCore
{
    public class WebManager : MonoBehaviour, IModulesHandler
    {
        [SerializeField] private GameObject webGameObject;

        private IUrlLoader _urlLoader;
        private ParametersManager _paramsManager;
        private IWebViewClient _webViewClient;
        private Stopwatch _stopwatch;

        public event Action Completed;

        public void PrepareForWork()
        {
            Debug.Log($"WebManagerMediator Init");
            _stopwatch = Stopwatch.StartNew();
            
            _urlLoader = webGameObject.gameObject.GetComponent<IUrlLoader>();
            _paramsManager = webGameObject.gameObject.GetComponent<ParametersManager>();
            _webViewClient = webGameObject.gameObject.GetComponent<IWebViewClient>();

            _paramsManager.PrepareForWork();
            
            _urlLoader.LoadingSucceeded += (s) => _paramsManager.DoWork();
            _paramsManager.Completed += StartWebview;
        }

        public void ResolveDependencies(ModulesOwner owner)
        {
            owner.Add(_urlLoader, _paramsManager, _webViewClient);
            _paramsManager.ResolveDependencies(owner);
            _webViewClient.ResolveDependencies(owner);
        }

        public void DoWork()
        {
            Debug.Log($"WebManagerMediator DoWork");
            _urlLoader.DoRequest();
        }
        
        private void StartWebview()
        {
            Debug.Log("WebManagerMediator StartWebview");
            string url = WebHelper.AttachParameters(_urlLoader.GetUrl(), _paramsManager.GetParams());
            if (WebHelper.IsValidUrl(url))
            {
                Debug.Log("open final url " + url);
                _webViewClient.SetSettings();
                _webViewClient.Open(url);
            }
            else
            {
                Debug.Log("not valid url " + url);
            }
            
            Debug.Log($"WebManagerMediator Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            _stopwatch.Stop();
        }
    }
}