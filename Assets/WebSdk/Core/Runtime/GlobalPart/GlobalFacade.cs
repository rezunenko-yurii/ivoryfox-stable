using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;
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
        public static ITrackingProvider TrackingProvider;
        //public static IAppTransparencyTracker Att;

        private static GameObject GlobalGameObject;

        public static void Init(GameObject globalGameObject)
        {
            GlobalGameObject = globalGameObject;
            MonoBehaviour = globalGameObject.GetComponent<MonoBehaviour>();

            Logger = GlobalGameObject.gameObject.GetComponent<ILogger>() ?? GlobalGameObject.gameObject.AddComponent<DummyLogger>();
            InternetChecker = GlobalGameObject.gameObject.GetComponent<IInternetChecker>() ?? GlobalGameObject.gameObject.AddComponent<DummyInternetChecker>();
            ConfigsLoader = GlobalGameObject.gameObject.GetComponent<IConfigsLoader>() ?? GlobalGameObject.gameObject.AddComponent<DummyConfigLoader>();
            Notification = GlobalGameObject.gameObject.GetComponent<INotification>() ?? GlobalGameObject.gameObject.AddComponent<DummyNotificationsClient>();
            TrackingProvider = GlobalGameObject.gameObject.GetComponent<ITrackingProvider>() ?? GlobalGameObject.gameObject.AddComponent<DummyTrackingProvider>();
            //Att = GlobalGameObject.gameObject.GetComponent<IAppTransparencyTracker>() ?? GlobalGameObject.gameObject.AddComponent<DummyAppTrackingTransparency>();
        }
    }
}