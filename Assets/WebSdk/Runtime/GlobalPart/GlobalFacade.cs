using GlobalBlock.Interfaces.Notifications;
using UnityEngine;

namespace GlobalBlock.Interfaces
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