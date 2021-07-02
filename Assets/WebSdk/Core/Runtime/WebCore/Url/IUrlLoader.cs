using System;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    public interface IUrlLoader: IModule, IConfigConsumer
    {
        event Action<string> LoadingFailed;
        event Action<string> LoadingSucceeded;
        void DoRequest();
        string GetUrl();
        void RemoveListeners();
        void OnGetResponse(string response);
    }
}
