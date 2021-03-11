using GlobalBlock.Interfaces.Notifications;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;

namespace GlobalBlock.Interfaces
{
    public interface IGlobalFactory
    {
        GameObject GameObject { get; set; }
        ILogger CreateLogger();
        IInternetChecker CreateInternetChecker();
        IConfigsLoader CreateConfigLoader();
        INotification CreateNotifications();
        IWebBlock CreateWebBlock();
    }
}