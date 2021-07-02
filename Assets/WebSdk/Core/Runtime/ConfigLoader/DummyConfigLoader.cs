using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public class DummyConfigLoader : MonoBehaviour, IConfigsLoader
    {
        public event Action<Dictionary<string, string>> Completed;

        public void Load(string configName)
        {
            Debug.Log("-------------- DummyConfigLoader Load /// !!!!!!!!!!!!! return empty data");
            Completed?.Invoke(new Dictionary<string, string>());
            Completed = null;
        }

        public void Load(List<string> configNames)
        {
            Debug.Log("-------------- DummyConfigLoader Load /// !!!!!!!!!!!!! return empty data");
            Completed?.Invoke(new Dictionary<string, string>());
            Completed = null;
        }
    }
}