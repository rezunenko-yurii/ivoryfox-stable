using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;
using WebSdk.Core.Runtime.AppTransparencyTrackers;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.Logger;
using WebSdk.Core.Runtime.Notifications;
using ILogger = WebSdk.Core.Runtime.Logger.ILogger;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public sealed class GlobalFacade
    {
        public static ILogger Logger;
        public static IInternetChecker InternetChecker;
        public static IConfigsLoader ConfigsLoader;
        public static INotification Notification;
        public static MonoBehaviour MonoBehaviour;
        public static IAdjustHelper AdjustHelper;
        public static IAppTransparencyTracker Att;

        static GlobalFacade()
        {
            Logger = new DummyLogger();
            InternetChecker = new DummyInternetChecker();
            ConfigsLoader = new DummyConfigLoader();
            Notification = new DummyNotificationsClient();
            AdjustHelper = new DummyAdjustHelper();
            Att = new DummyAppTrackingTransparency();
        }

        public static void Init(ILogger logger, IInternetChecker internetChecker, IConfigsLoader configsLoader, INotification notification, IAdjustHelper adjustHelper, IAppTransparencyTracker att)
        {
            Logger = logger;
            InternetChecker = internetChecker;
            ConfigsLoader = configsLoader;
            Notification = notification;
            AdjustHelper = adjustHelper;
            Att = att;
        }

        public static void Init(IGlobalFactory factory, MonoBehaviour monoBehaviour)
        {
            AdjustHelper = factory.CreateAdjustHelper();
            Logger = factory.CreateLogger();
            InternetChecker = factory.CreateInternetChecker();
            ConfigsLoader = factory.CreateConfigLoader();
            Notification = factory.CreateNotifications();
            Att = factory.CreateAppTransparencyTracker();

            MonoBehaviour = monoBehaviour;
        }
    }
}