﻿using System.Collections.Generic;
using System.Diagnostics;
using Global.Helpers.Runtime;
using GlobalBlock.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.Notifications;
using GlobalBlock.Interfaces.WebPart;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
using ILogger = GlobalBlock.Interfaces.ILogger;

namespace WebSdkRealizations.DefaultAccessManager.Runtime.Scripts
{
    public class AccessManager : MonoBehaviour, IGlobalBlock
    {
        public TextMeshProUGUI textfield;
        private ILogger Logger { get; set; }
        private IInternetChecker InternetChecker { get; set; }
        private IConfigsLoader ConfigLoader { get; set; }
        private INotification Notification { get; set; }
        private IWebBlock _webBlock;
        private Stopwatch _stopwatch;

        #region Initialization
        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");
            AccessManager[] systems = FindObjectsOfType<AccessManager>();
            
            if (systems.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(this);
            
            _stopwatch = Stopwatch.StartNew();
            
            //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            var factory = new AccessManagerComponentsFactory {GameObject = gameObject};
            InitModules(factory);
            
            InternetChecker.OnResult += TryLoadConfigs;
            InternetChecker.Check(3);
        }
        

        public void InitModules(IGlobalFactory factory)
        {
            Debug.Log("GlobalBlockUnity InitModules");
            
            Logger = factory.CreateLogger();
            InternetChecker = factory.CreateInternetChecker();
            ConfigLoader = factory.CreateConfigLoader();
            Notification = factory.CreateNotifications();
            _webBlock = factory.CreateWebBlock();
            
            ConnectToFacade();
        }
        
        public void ConnectToFacade()
        {
            Debug.Log("GlobalBlockUnity ConnectToFacade");
            
            GlobalFacade.logger = Logger;
            GlobalFacade.internetChecker = InternetChecker;
            GlobalFacade.configsLoader = ConfigLoader;
            GlobalFacade.notification = Notification;
            GlobalFacade.monoBehaviour = this;
        }
        
        #endregion

        #region Cofigs
        
        public void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"GlobalBlockUnity LoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection)
            {
                InternetChecker.OnResult -= TryLoadConfigs;

                LoadConfigs();
            }
            else
            {
                CheckRepeatsLeft();
            }
        }

        private void LoadConfigs()
        {
            List<string> ids = Helper.GetConsumableIds(Logger, Notification);
            ids.Add("canUse");

            if (ids.Count > 0)
            {
                ConfigLoader.Load(ids, InitConfigs);
            }
            else
            {
                Helper.LoadNextScene();
            }
        }
        
        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"GlobalBlockUnity InitConfigs / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);
            if (canUse || ConfigLoader is RemoteConfigsLoader)
            {
                Helper.SetConfigsToConsumables(configs, Logger, Notification);
                Helper.LoadNextScene();
            
                Debug.Log($"GlobalBlockUnity Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
                _stopwatch.Stop();
            
                _webBlock.Init();
            }
            else
            {
                Debug.Log($"GlobalBlockUnity InitConfigs / canUse = false");
                Helper.LoadNextScene();
            }
        }
        
        #endregion

        private void CheckRepeatsLeft()
        {
            if (InternetChecker.RepeatsLeft() > 0)
            {
                textfield.text = "No internet connection. \n Please turn on the internet or wait ";
            }
            else
            {
                Helper.LoadNextScene();
            }
        }
    }
}