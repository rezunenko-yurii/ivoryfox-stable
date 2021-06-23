using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.Tracking;
using WebSdk.Tracking.Runtime.Scripts;
using Debug = UnityEngine.Debug;

namespace WebSdk.Main.Runtime.Scripts
{
    public class WebSdkEntry : MonoBehaviour, IModulesHost
    {
        [SerializeField] private GameObject globalGameObject;
        [SerializeField] private GameObject webGameObject;
        [SerializeField] private GameObject trackingGameObject;
        
        public TextMeshProUGUI textfield;
        
        [SerializeField] private WebManager _webManager;
        [SerializeField] private GlobalComponentsManager _globalManager;
        [SerializeField] private TrackingManager _trackingManager;
        private Stopwatch _stopwatch;

        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");
            
            _trackingManager.InitModules(trackingGameObject, this);
            _globalManager.InitModules(globalGameObject, this);
            _webManager.InitModules(webGameObject, this);

            ModulesNavigation.SetWebBlockSettings();
            
            _stopwatch = Stopwatch.StartNew();
            
            CheckAtt();
        }
        
        private void CheckAtt()
        {
            Debug.Log("WebSdkEntry CheckAtt");
            
            _trackingManager.Att.OnGetRequest += CheckInternetConnection;
            _trackingManager.Att.DoRequest();
        }

        private void CheckInternetConnection()
        {
            Debug.Log("WebSdkEntry CheckInternetConnection");
            
            _globalManager.InternetChecker.OnRepeatCheckResult += ChangeLoaderText;
            _globalManager.InternetChecker.OnRepeatEndResult += TryLoadConfigs;

            _globalManager.InternetChecker.Check(3);
        }

        #region Cofigs
        
        private void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"WebSdkEntry TryLoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection) LoadConfigs();
            else
            {
                Debug.Log($"WebSdkEntry -> No internet connection -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
        }
        
        private void ChangeLoaderText(bool hasConnection)
        {
            Debug.Log($"WebSdkEntry TryLoadConfigs / hasConnection {hasConnection}");
            
            const string noInternetText = "No internet connection. \n Please turn on the internet or wait ";
            const string loadingText = "Loading...";
            
            textfield.text = hasConnection ? loadingText : noInternetText;
        }

        private void LoadConfigs()
        {
            Debug.Log($"WebSdkEntry LoadConfigs");

            var ids = new List<string> {"canUse"};
            ids = ids.Union(_globalManager.GetConfigIds()).ToList();
            ids = ids.Union(_trackingManager.GetConfigIds()).ToList();
            ids = ids.Union(_webManager.GetConfigIds()).ToList();

            if (ids.Count > 0) _globalManager.ConfigsLoader.Load(ids, InitConfigs);
            else
            {
                Debug.Log($"WebSdkEntry -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
        }
        
        private void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"WebSdkEntry InitConfigs / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");

            SetLoadedDataToModules(configs);

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);

            if (canUse)
            {
                Debug.Log($"GlobalBlockUnity Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
                _webManager.DoWork();
            }
            else
            {
                Debug.Log($"WebSdkEntry / canUse = false -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
            
            _stopwatch.Stop();
        }

        private void SetLoadedDataToModules(Dictionary<string, string> configs)
        {
            List<IModule> modules = new List<IModule>();
            modules = modules.Union(_globalManager.GetModulesForConfigs()).ToList();
            modules = modules.Union(_webManager.GetModulesForConfigs()).ToList();
            modules = modules.Union(_trackingManager.GetModulesForConfigs()).ToList();

            ConfigLoaderHelper.SetConfigsToConsumables(configs, modules.ToArray());
        }

        #endregion
        
        private void OnDestroy()
        {
            Debug.Log($"WebSdkEntry OnDestroy");
            _globalManager.Logger.Clear();
        }

        public Dictionary<Type, IModule> Modules { get; set; } = new Dictionary<Type, IModule>();
        public IModulesHost Parent { get; set; }

        public IModule GetModule(Type moduleType)
        {
            if (Modules.ContainsKey(moduleType))
            {
                return Modules[moduleType];
            }
            else
            {
                return null;
            }
        }

        public void AddModule(Type moduleType, IModule module)
        {
            if (!Modules.ContainsKey(moduleType))
            {
                Modules.Add(moduleType, module);
            }
        }
    }
}