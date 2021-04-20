using System;
using System.Collections.Generic;
using GlobalBlock.Interfaces;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IParamsManager : IModule, IConfigConsumer, IMediatorComponent
    {
        event Action<Dictionary<string,string>> OnAllReady;
        event Action<string> OnError;
        event Action OnComplete;

        void Init();

        Dictionary<string, string> GetParams();

    }
}