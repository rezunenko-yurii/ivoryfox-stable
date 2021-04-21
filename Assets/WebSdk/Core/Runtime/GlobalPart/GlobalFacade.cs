﻿using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Logger;
using WebSdk.Core.Runtime.Notifications;
using ILogger = WebSdk.Core.Runtime.Logger.ILogger;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public sealed class GlobalFacade
    {
        public static ILogger logger;
        public static IInternetChecker internetChecker;
        public static IConfigsLoader configsLoader;
        public static INotification notification;
        public static MonoBehaviour monoBehaviour;

        static GlobalFacade()
        {
            logger = new DummyLogger();
            internetChecker = new DummyInternetChecker();
            configsLoader = new DummyConfigLoader();
            notification = new DummyNotificationsClient();
        }
    }
}