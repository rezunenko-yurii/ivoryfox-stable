using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using Debug = UnityEngine.Debug;

namespace WebSdk.Core.Runtime.WebCore
{
    public class WebManager : MonoBehaviour, IModulesManager, IMediator
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

            _urlLoader.SetMediator(this);
            _paramsManager.SetMediator(this);
            _webViewClient.SetMediator(this);
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

        public void Notify(object sender, string ev)
        {
            if (ev.Equals("OnUrlLoaded"))
            {
                _paramsManager.Init();
            }
            else if (ev.Equals("OnParamsLoaded"))
            {
                StartWebview();
            }
            else if (ev.Equals("Error"))
            {
                Debug.Log("WebManagerMediator Notify catch error");
            }
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