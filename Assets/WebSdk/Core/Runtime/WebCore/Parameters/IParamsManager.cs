using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public interface IParamsManager : IConfigConsumer, IMediatorComponent, IModulesHost
    {
        event Action<string> OnError;
        event Action OnComplete;
        void Init();
        Dictionary<string, string> GetParams();
    }
}