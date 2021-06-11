using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebViewClient: IModule, IMediatorComponent
    {
        void Open(string url);
        void SetSettings();
    }
}