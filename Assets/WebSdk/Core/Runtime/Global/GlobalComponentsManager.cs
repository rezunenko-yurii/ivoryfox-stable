using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Logger;

namespace WebSdk.Core.Runtime.Global
{
    public class GlobalComponentsManager : MonoBehaviour, IModulesHandler
    {
        [SerializeField] private GameObject globalGameObject;
        
        public event Action Completed;
        
        private ICustomLogger _logger;
        private IInternetChecker _internetChecker;
        private IConfigsLoader _configsLoader;
        private ModulesOwner _modulesOwner;

        public void PrepareForWork()
        {
            _logger = globalGameObject.gameObject.GetComponent<ICustomLogger>() ?? globalGameObject.gameObject.AddComponent<DummyCustomLogger>();
            _internetChecker = globalGameObject.gameObject.GetComponent<IInternetChecker>() ?? globalGameObject.gameObject.AddComponent<DummyInternetChecker>();
            _configsLoader = globalGameObject.gameObject.GetComponent<IConfigsLoader>() ?? globalGameObject.gameObject.AddComponent<DummyConfigLoader>();
        }

        public void ResolveDependencies(ModulesOwner owner)
        {
            _modulesOwner = owner;
            owner.Add(_logger, _internetChecker, _configsLoader);
        }

        public void DoWork()
        {
            CheckInternetConnection();
        }
        
        private void CheckInternetConnection()
        {
            Debug.Log("WebSdkEntry CheckInternetConnection");
            
            //InternetChecker.Checked += ChangeLoaderText;
            _internetChecker.RepeatsEnded += TryLoadConfigs;
            _internetChecker.Check(3);
        }
        
        private void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"WebSdkEntry TryLoadConfigs / hasConnection {hasConnection}");

            if (hasConnection)
            {
                LoadConfigs();
            }
            else
            {
                Debug.Log($"WebSdkEntry -> No internet connection -> GoToNativeBlock");
                GameNavigation.GoToNativeBlock();
            }
        }
        
        private void LoadConfigs()
        {
            Debug.Log($"WebSdkEntry LoadConfigs");

            var configsHandlers = _modulesOwner.GetAll<IConfigConsumer>();
            var ids = ConfigLoaderHelper.GetConsumableIds(configsHandlers);
            ids.Add("canUse");

            if (configsHandlers.Count > 0)
            {
                _configsLoader.Completed += InitConfigs;
                _configsLoader.Load(ids);
            }
            else
            {
                Debug.Log($"WebSdkEntry -> GoToNativeBlock");
                GameNavigation.GoToNativeBlock();
            }
        }
        
        private void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"WebSdkEntry InitConfigs");

            SetLoadedDataToModules(configs);

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);

            if (canUse)
            {
                Debug.Log($"GlobalBlockUnity Complete");
                //_webManager.DoWork();
                Completed?.Invoke();
                Completed = null;
            }
            else
            {
                Debug.Log($"WebSdkEntry / canUse = false -> GoToNativeBlock");
                GameNavigation.GoToNativeBlock();
            }
        }
        
        private void SetLoadedDataToModules(Dictionary<string, string> configs)
        {
            var configsHandlers = _modulesOwner.GetAll<IConfigConsumer>();
            ConfigLoaderHelper.SetConfigsToConsumables(configs, configsHandlers);
        }
    }
}