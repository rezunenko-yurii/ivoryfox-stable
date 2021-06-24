using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public class DummyConfigLoader : MonoBehaviour, IConfigsLoader
    {
        public void Load(string configName, Action<Dictionary<string, string>> onComplete)
        {
            Debug.Log("-------------- DummyConfigLoader Load /// !!!!!!!!!!!!! return empty data");
            onComplete?.Invoke(new Dictionary<string, string>());
        }

        public void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete)
        {
            Debug.Log("-------------- DummyConfigLoader Load /// !!!!!!!!!!!!! return empty data");
            onComplete?.Invoke(new Dictionary<string, string>());
        }

        public IModulesHost Parent { get; set; }
    }
}