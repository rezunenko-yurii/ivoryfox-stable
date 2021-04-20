namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebFactory
    {
        IUrlLoader CreateUrlLoader();
        IParamsManager CreateParamsManager();
        IWebViewClient CreateWebViewClient();
        IWebMediator CreateMediator();
    }
}