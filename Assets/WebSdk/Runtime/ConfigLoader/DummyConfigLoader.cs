using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalBlock.Interfaces
{
    public class DummyConfigLoader : IConfigsLoader
    {
        public void Load(string configName, Action<Dictionary<string, string>> onComplete)
        {
            onComplete?.Invoke(new Dictionary<string, string>());
        }

        public void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete)
        {
            onComplete?.Invoke(new Dictionary<string, string>());
        }
    }
}