using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.WebCore.WebView
{
    public interface IWebViewClient: IModule
    {
        void Open(string url);
        void SetSettings();
    }
}