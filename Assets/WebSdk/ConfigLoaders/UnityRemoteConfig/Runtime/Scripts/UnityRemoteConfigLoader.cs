using System;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts
{
    public class UnityRemoteConfigLoader : MonoBehaviour, IConfigsLoader
    {
        private struct userAttributes {}
        private struct appAttributes {}

        private List<string> configNames;

        public event Action<Dictionary<string, string>> Completed;

        public void Load(string configName)
        {
            Debug.Log($"UnityConfigsLoader Load {configName}");
            configNames = new List<string> {configName};

            FetchConfigs();
        }

        public void Load(List<string> configNames)
        {
            string cn = string.Empty;
            foreach (string configName in configNames)
            {
                cn += " ";
                cn += configName;
            }
            Debug.Log($"UnityConfigsLoader Load {cn}");
            
            this.configNames = configNames;

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
            var dict = ReturnConfigs();
            
            Completed?.Invoke(dict);
            Completed = null;
        }
        
        private Dictionary<string, string> ReturnConfigs()
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

            return dict;
        }
    }
}
