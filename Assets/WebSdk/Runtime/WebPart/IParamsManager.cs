using System;
using System.Collections.Generic;

namespace GlobalBlock.Interfaces.WebPart
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