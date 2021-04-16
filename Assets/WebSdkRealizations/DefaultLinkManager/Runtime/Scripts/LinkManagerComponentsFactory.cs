using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using WebSdkExtensions.Parameters.Runtime.Scripts;
using WebSdkExtensions.UrlLoaders.Unity.Scripts;
using WebSdkExtensions.WebViewClients.BrowserWebview.Runtime;

namespace WebBlock.WebBlockVariant1.Scripts
{
    public class LinkManagerComponentsFactory : IWebFactory
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
#if UNITY_EDITOR
            return new BrowserWebviewClient();
#else
            return new UniWebViewClient();
#endif
}

public IWebMediator CreateMediator()
{
return new LinkManagerMediator();
}
}
}