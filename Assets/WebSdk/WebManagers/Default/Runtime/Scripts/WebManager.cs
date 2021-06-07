﻿using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.WebCore;
using Debug = UnityEngine.Debug;

namespace WebSdk.WebManagers.Default.Runtime.Scripts
{
    public class WebManager: IWebManager
    {
        public IUrlLoader UrlLoader { get; private set; }
        public IParamsManager ParamsManager { get; private set; }
        public IWebViewClient WebViewClient { get; private set; }
        private IWebMediator _mediator;
        private Stopwatch _stopwatch;

        public void Init()
        {
            _stopwatch = Stopwatch.StartNew();
            Debug.Log("WebVariant1 Init");
            
            InitModules(new WebManagerComponentsFactory());
            LoadConfigs();
        }

        public void InitModules(IWebFactory factory)
        {
            Debug.Log("WebVariant1 InitModules");
            
            _mediator = factory.CreateMediator();
            UrlLoader = factory.CreateUrlLoader();
            ParamsManager = factory.CreateParamsManager();
            WebViewClient = factory.CreateWebViewClient();

            _mediator.Init(UrlLoader, ParamsManager, WebViewClient);
        }

        public void LoadConfigs()
        {
            Debug.Log("Load Web Configs");
            
            List<string> ids = ConfigLoaderHelper.GetConsumableIds(UrlLoader, ParamsManager, WebViewClient);

            if (ids.Count > 0)
            {
                GlobalFacade.ConfigsLoader.Load(ids, InitConfigs);
            }
            else
            {
                Debug.Log("WebVariant1 // There is no configs to load / load next scene");
                SceneHelper.LoadNextScene();
            }
        }

        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log("WebVariant1 InitConfigs");
            
            ConfigLoaderHelper.SetConfigsToConsumables(configs, UrlLoader, ParamsManager);
            Debug.Log($"WebVariant1 Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            _stopwatch.Stop();
            
            _mediator.Start();
        }
    }
}