using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Notifications;
using WebSdk.Core.Runtime.WebCore;
using ILogger = WebSdk.Core.Runtime.Logger.ILogger;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IGlobalFactory
    {
        GameObject GameObject { get; set; }
        ILogger CreateLogger();
        IInternetChecker CreateInternetChecker();
        IConfigsLoader CreateConfigLoader();
        INotification CreateNotifications();
        IWebManager CreateWebBlock();
        IAdjustHelper CreateAdjustHelper();
    }
}