﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore.Parameters;
using WebSdk.Core.Runtime.WebCore.Url;
using WebSdk.Core.Runtime.WebCore.WebView;
using Debug = UnityEngine.Debug;

namespace WebSdk.Core.Runtime.WebCore
{
    public class WebManager : MonoBehaviour, IModulesManager
    {
        private IUrlLoader _urlLoader;
        private IParamsManager _paramsManager;
        private IWebViewClient _webViewClient;
        private Stopwatch _stopwatch;

        public List<string> GetConfigIds()
        {
            return ConfigLoaderHelper.GetConsumableIds(_urlLoader, _paramsManager, _webViewClient);
        }

        public List<IModule> GetModulesForConfigs()
        {
            return new List<IModule> {_urlLoader, _paramsManager, _webViewClient};
        }

        public GameObject HostGameObject { get; private set; }

        public void InitModules(GameObject webGameObject, IModulesHost parent)
        {
            Debug.Log($"WebManagerMediator Init");
            _stopwatch = Stopwatch.StartNew();

            HostGameObject = webGameObject;
            Parent = parent;

            _urlLoader = HostGameObject.gameObject.GetComponent<IUrlLoader>();
            _paramsManager = HostGameObject.gameObject.GetComponent<IParamsManager>();
            _webViewClient = HostGameObject.gameObject.GetComponent<IWebViewClient>();

            _urlLoader.Parent = this;
            _paramsManager.Parent = this;
            _webViewClient.Parent = this;

            Modules = Parent.Modules;

            Modules.Add(_urlLoader.GetType(), _urlLoader);
            Modules.Add(_paramsManager.GetType(), _paramsManager);
            Modules.Add(_webViewClient.GetType(), _webViewClient);

            _urlLoader.LoadingSucceeded += (s) => _paramsManager.Init();
            _paramsManager.Completed += StartWebview;
        }

        public Dictionary<Type, IModule> Modules { get; set; }
        public IModulesHost Parent { get; set; }

        public void DoWork()
        {
            Debug.Log($"WebManagerMediator DoWork");
            _urlLoader.DoRequest();
        }

        public IModule GetModule(Type moduleType)
        {
            return Parent.GetModule(moduleType);
        }

        public void AddModule(Type moduleType, IModule module)
        {
            Parent.AddModule(moduleType, module);
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