﻿using GlobalBlock.Interfaces;
using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Notifications;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.InternetCheckers.Default.Runtime.Scripts;
using WebSdk.WebManagers.Default.Runtime.Scripts;
using WebSdkExtensions.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts;
using WebSdkExtensions.Loggers.Scripts;
using ILogger = GlobalBlock.Interfaces.ILogger;

namespace WebSdk.SdkConfigurations.RemoteConfigBased.Runtime.Scripts
{
    [CreateAssetMenu(fileName = "WebSdkComponentsFactory", menuName = "IvoryFox/Create/WebSdk/RemoteConfigBasedFactory", order = 0)]
    public class RemoteConfigBasedFactory: ScriptableObject, IGlobalFactory
    {
        public GameObject GameObject { get; set; }

        public ILogger CreateLogger()
        {
            return new RemoteLogger();
        }

        public IInternetChecker CreateInternetChecker()
        {
            return GameObject.AddComponent<DefaultInternetChecker>();
        }

        public IConfigsLoader CreateConfigLoader()
        {
            return new RemoteConfigsLoader();
        }

        public INotification CreateNotifications()
        {
            return new DummyNotificationsClient();
        }

        public IWebManager CreateWebBlock()
        {
            return new WebManager();
        }
    }
}