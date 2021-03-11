using System;
using System.Collections.Generic;
using System.Diagnostics;
using Global.Helpers.Runtime;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace WebBlock.WebBlockVariant1.Scripts
{
    public class LinkManager: IWebBlock
    {
        public IUrlLoader UrlLoader { get; private set; }
        public IParamsManager ParamsManager { get; private set; }
        public IWebViewClient WebViewClient { get; private set; }
        private IWebMediator mediator;
        private Stopwatch stopwatch;

        public void Init()
        {
            stopwatch = Stopwatch.StartNew();
            Debug.Log("WebVariant1 Init");
            
            InitModules(new LinkManagerComponentsFactory());
            LoadConfigs();
        }

        public void InitModules(IWebFactory factory)
        {
            Debug.Log("WebVariant1 InitModules");
            
            mediator = factory.CreateMediator();
            UrlLoader = factory.CreateUrlLoader();
            ParamsManager = factory.CreateParamsManager();
            WebViewClient = factory.CreateWebViewClient();

            mediator.Init(UrlLoader, ParamsManager, WebViewClient);
        }

        public void LoadConfigs()
        {
            Debug.Log("Load Web Configs");
            
            List<string> ids = Helper.GetConsumableIds(UrlLoader, ParamsManager, WebViewClient);

            if (ids.Count > 0)
            {
                GlobalFacade.configsLoader.Load(ids, InitConfigs);
            }
            else
            {
                Debug.Log("WebVariant1 // There is no configs to load / load next scene");
                Helper.LoadNextScene();
            }
        }

        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log("WebVariant1 InitConfigs");
            
            Helper.SetConfigsToConsumables(configs, UrlLoader, ParamsManager);
            Debug.Log($"WebVariant1 Complete / StopWatch = {stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            stopwatch.Stop();
            
            mediator.Start();
        }
    }
}