using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.Notifications;
using UnityEngine;
using WebSdk.Core.Runtime.Notifications;
using WebSdk.Core.Runtime.WebCore;
using ILogger = GlobalBlock.Interfaces.ILogger;

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
    }
}