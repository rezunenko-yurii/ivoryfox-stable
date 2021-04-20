using GlobalBlock.Interfaces;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebMediator : IMediator
    {
        void Init(IUrlLoader u, IParamsManager p, IWebViewClient w);
    }
}