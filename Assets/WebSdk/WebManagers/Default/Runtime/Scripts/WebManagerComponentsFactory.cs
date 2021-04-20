using WebSdk.Core.Runtime.WebCore;
using WebSdk.Extensions.UrlLoaders.Unity.Runtime.Scripts;
using WebSdk.WebViewClients.Browser.Runtime.Scripts;
using WebSdk.WebViewClients.UniWebView.Runtime.Scripts;
using WebSdkExtensions.Parameters.Runtime.Scripts;

namespace WebSdk.WebManagers.Default.Runtime.Scripts
{
    public class WebManagerComponentsFactory : IWebFactory
    {
        public IUrlLoader CreateUrlLoader()
        {
            return new UnityUrlLoader();
        }

        public IParamsManager CreateParamsManager()
        {
            return new ParametersManager();
        }

        public IWebViewClient CreateWebViewClient()
        {
#if UNITY_EDITOR && UNITY_EDITOR_WIN
            return new BrowserWebviewClient();
#else
            return new UniWebViewClient();
#endif
        }

        public IWebMediator CreateMediator()
        {
            return new WebManagerMediator();
        }
    }
}