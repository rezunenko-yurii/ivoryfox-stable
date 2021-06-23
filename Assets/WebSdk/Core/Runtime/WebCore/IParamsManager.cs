using System;
using System.Collections.Generic;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IParamsManager : IModule, IConfigConsumer, IMediatorComponent
    {
        event Action<string> OnError;
        event Action OnComplete;

        void Init();
        Dictionary<string, string> GetParams();
    }
}