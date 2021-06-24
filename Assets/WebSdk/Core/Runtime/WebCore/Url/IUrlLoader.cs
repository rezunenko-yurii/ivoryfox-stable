using System;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.Url
{
    public interface IUrlLoader: IModule, IConfigConsumer, IMediatorComponent
    {
        event Action<string> OnFailure;
        event Action<string> OnSuccess;
        void DoRequest();
        string GetUrl();
        void RemoveListeners();
        void OnGetResponse(string response);
    }
}
