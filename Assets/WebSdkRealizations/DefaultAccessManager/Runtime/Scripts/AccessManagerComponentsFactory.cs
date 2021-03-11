using GlobalBlock.ConfigLoaders.CloudContentDelivery.Runtime.Scripts;
using GlobalBlock.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.Notifications;
using GlobalBlock.Interfaces.WebPart;
using GlobalBlock.Notifications.UnityAndroidNotifications.Scripts;
using InternetCheckers.Scripts;
using Loggers.Scripts;
using UnityEngine;
using WebBlock.WebBlockVariant1.Scripts;
using ILogger = GlobalBlock.Interfaces.ILogger;

namespace GlobalBlock.GlobalUnityVariant.Scripts
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
            return new CloudContentLoader();
        }

        public INotification CreateNotifications()
        {
            return new UnityAndroidNotificationsClient();
        }

        public IWebBlock CreateWebBlock()
        {
            return new LinkManager();
        }
    }
}