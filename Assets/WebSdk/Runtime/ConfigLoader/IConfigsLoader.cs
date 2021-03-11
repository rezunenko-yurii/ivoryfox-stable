using System;
using System.Collections.Generic;

namespace GlobalBlock.Interfaces
{
    public interface IConfigsLoader: IModule
    {
        void Load(string configName, Action<Dictionary<string, string>> onComplete);
        void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete);
    }
}