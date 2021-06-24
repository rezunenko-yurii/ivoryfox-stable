using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.ConfigLoader
{
    public interface IConfigsLoader: IModule
    {
        void Load(string configName, Action<Dictionary<string, string>> onComplete);
        void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete);
    }
}