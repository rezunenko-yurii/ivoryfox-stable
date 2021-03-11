using System.Collections.Generic;

namespace GlobalBlock.Interfaces.WebPart
{
    public interface IWebBlock
    {
        IUrlLoader UrlLoader { get; }
        IParamsManager ParamsManager { get; }
        IWebViewClient WebViewClient { get; }
        
        void Init();
        void InitModules(IWebFactory factory);
        void LoadConfigs();
        void InitConfigs(Dictionary<string, string> configs);
    }
}