using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public interface IConfigsLoader: IModule
    {
        event Action<Dictionary<string, string>> Completed;
        void Load(string configName);
        void Load(List<string> configNames);
    }
}