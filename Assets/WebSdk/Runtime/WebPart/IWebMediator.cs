namespace GlobalBlock.Interfaces.WebPart
{
    public interface IWebMediator : IMediator
    {
        void Init(IUrlLoader u, IParamsManager p, IWebViewClient w);
    }
}