using System;
using System.Collections.Generic;
using GlobalBlock.Interfaces;

namespace WebSdkExtensions.ConfigLoaders.UniWebRequest.Scripts
{
    public class UnityWebRequestLinkLoader : IConfigsLoader
    {
        public void Load(string configName, Action<Dictionary<string, string>> onComplete)
        {
            throw new NotImplementedException();
        }

        public void Load(List<string> configNames, Action<Dictionary<string, string>> onComplete)
        {
            throw new NotImplementedException();
        }
    }
}