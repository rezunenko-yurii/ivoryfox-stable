using System;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;

namespace WebSdk.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts
{
    public class RemoteConfigsLoader : IConfigsLoader
    {
        private struct userAttributes {}
        private struct appAttributes {}

        private List<string> configNames;
        private Action<Dictionary<string, string>> callback;

        public void Load(string configName, Action<Dictionary<string, string>> onComplete)
        {
            Debug.Log($"UnityConfigsLoader Load {configName}");
            configNames = new List<string> {configName};
            callback = onComplete;
            
            FetchConfigs();
        }

        public void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete)
        {
            string cn = string.Empty;
            foreach (string configName in configNames)
            {
                cn += " ";
                cn += configName;
            }
            Debug.Log($"UnityConfigsLoader Load {cn}");
            
            this.configNames = configNames;
            callback = onComplete;
            
            FetchConfigs();
        }
        
        private void FetchConfigs()
        {
            Debug.Log($"UnityConfigsLoader FetchConfigs");
            
            ConfigManager.FetchCompleted += OnConfigsFetched;
            ConfigManager.FetchConfigs(new userAttributes(), new appAttributes());
        }
        
        void OnConfigsFetched (ConfigResponse configResponse) 
        {
            Debug.Log($"UnityConfigsLoader {nameof(OnConfigsFetched)} configResponse {configResponse.requestOrigin}");
            
            ConfigManager.FetchCompleted -= OnConfigsFetched;
            ReturnConfigs();
        }
        
        private void ReturnConfigs()
        {
            var dict = new Dictionary<string, string>();

            foreach (string configName in configNames)
            {
                if (!string.IsNullOrEmpty(configName))
                {
                    Debug.Log($"Return config {configName} ----- {ConfigManager.appConfig.GetJson(configName)}");
                    dict.Add(configName, ConfigManager.appConfig.GetJson(configName));
                }
            }

            callback.Invoke(dict);
        }
    }
}
