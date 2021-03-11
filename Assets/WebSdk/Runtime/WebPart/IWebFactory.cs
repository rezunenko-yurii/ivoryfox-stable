namespace GlobalBlock.Interfaces.WebPart
{
    public interface IWebFactory
    {
        IUrlLoader CreateUrlLoader();
        IParamsManager CreateParamsManager();
        IWebViewClient CreateWebViewClient();
        IWebMediator CreateMediator();
    }
}