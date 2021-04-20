using GlobalBlock.Interfaces;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebViewClient: IModule, IMediatorComponent
    {
        void Open(string url);
        void SetSettings();
    }
}