using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.Notifications;
using GlobalBlock.Interfaces.WebPart;
using UnityEngine;
using WebBlock.WebBlockVariant1.Scripts;
using WebSdkExtensions.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts;
using WebSdkExtensions.InternetCheckers.Scripts;
using WebSdkExtensions.Loggers.Scripts;
using ILogger = GlobalBlock.Interfaces.ILogger;

namespace WebSdkRealizations.DefaultAccessManager.Runtime.Scripts
{
    public class AccessManagerComponentsFactory: IGlobalFactory
    {
        public GameObject GameObject { get; set; }

        public ILogger CreateLogger()
        {
            return new RemoteLogger();
        }

        public IInternetChecker CreateInternetChecker()
        {
            return GameObject.AddComponent<InternetChecker>();
        }

        public IConfigsLoader CreateConfigLoader()
        {
            return new RemoteConfigsLoader();
        }

        public INotification CreateNotifications()
        {
            return new DummyNotificationsClient();
        }

        public IWebBlock CreateWebBlock()
        {
            return new LinkManager();
        }
    }
}