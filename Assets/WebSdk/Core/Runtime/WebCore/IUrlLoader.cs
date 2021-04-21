using System;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IUrlLoader: IModule, IConfigConsumer, IMediatorComponent
    {
        event Action<string> OnFailure;
        event Action<string> OnSuccess;
        void Start();
        string GetUrl();
        void RemoveListeners();
        void OnGetResponse(string response);
    }
}
