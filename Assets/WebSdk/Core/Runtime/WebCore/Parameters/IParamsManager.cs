using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public interface IParamsManager : IConfigConsumer, IModulesHost
    {
        event Action<string> Failed;
        event Action Completed;
        void Init();
        Dictionary<string, string> GetParams();
    }
}