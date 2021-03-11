using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.WebPart;
using IvoryFox.WebSDK.Parameters;
using WebBlock.UrlLoaders.Unity.Scripts;
using WebBlock.WebViewClients.BrowserWebview.Runtime;

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