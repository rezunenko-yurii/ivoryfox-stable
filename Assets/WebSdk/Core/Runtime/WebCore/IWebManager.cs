using System.Collections.Generic;

namespace WebSdk.Core.Runtime.WebCore
{
    public interface IWebManager
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