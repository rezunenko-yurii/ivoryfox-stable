using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers.Scripts;
using WebSdk.Core.Runtime.InternetChecker;
using WebSdk.Core.Runtime.WebCore;
using Debug = UnityEngine.Debug;
using ILogger = WebSdk.Core.Runtime.Logger.ILogger;
using INotification = WebSdk.Core.Runtime.Notifications.INotification;

namespace WebSdk.Main.Runtime.Scripts
{
    public class WebSdkEntry : MonoBehaviour, IGlobalBlock
    {
        public TextMeshProUGUI textfield;
        private ILogger Logger { get; set; }
        private IInternetChecker InternetChecker { get; set; }
        private IConfigsLoader ConfigLoader { get; set; }
        private INotification Notification { get; set; }
        private IWebManager _webManager;
        private Stopwatch _stopwatch;

        #region Initialization
        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");
            WebSdkEntry[] systems = FindObjectsOfType<WebSdkEntry>();
            
            if (systems.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(this);
            
            _stopwatch = Stopwatch.StartNew();
            
            //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            var factory = Resources.Load<ScriptableObject>("WebSdkComponentsFactory") as IGlobalFactory; //new AccessManagerComponentsFactory {GameObject = gameObject};
            factory.GameObject = this.gameObject;
            InitModules(factory);
            
            InternetChecker.OnResult += TryLoadConfigs;
            InternetChecker.Check(3);
        }
        
        
        public void InitModules(IGlobalFactory factory)
        {
            Debug.Log("GlobalBlockUnity InitModules");
            
            Logger = factory.CreateLogger();
            InternetChecker = factory.CreateInternetChecker();
            ConfigLoader = factory.CreateConfigLoader();
            Notification = factory.CreateNotifications();
            _webManager = factory.CreateWebBlock();
            
            ConnectToFacade();
        }
        
        public void ConnectToFacade()
        {
            Debug.Log("GlobalBlockUnity ConnectToFacade");
            
            GlobalFacade.logger = Logger;
            GlobalFacade.internetChecker = InternetChecker;
            GlobalFacade.configsLoader = ConfigLoader;
            GlobalFacade.notification = Notification;
            GlobalFacade.monoBehaviour = this;
        }
        
        #endregion

        #region Cofigs
        
        public void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"GlobalBlockUnity LoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection)
            {
                InternetChecker.OnResult -= TryLoadConfigs;

                LoadConfigs();
            }
            else
            {
                CheckRepeatsLeft();
            }
        }

        private void LoadConfigs()
        {
            List<string> ids = Helper.GetConsumableIds(Logger, Notification);
            ids.Add("canUse");

            if (ids.Count > 0)
            {
                ConfigLoader.Load(ids, InitConfigs);
            }
            else
            {
                Helper.LoadNextScene();
            }
        }
        
        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"GlobalBlockUnity InitConfigs / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);
            
            //if (canUse || ConfigLoader is RemoteConfigsLoader)
            if (canUse)
            {
                Helper.SetConfigsToConsumables(configs, Logger, Notification);
                Helper.LoadNextScene();
            
                Debug.Log($"GlobalBlockUnity Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
                _stopwatch.Stop();
            
                _webManager.Init();
            }
            else
            {
                Debug.Log($"GlobalBlockUnity InitConfigs / canUse = false");
                Helper.LoadNextScene();
            }
        }
        
        #endregion

        private void CheckRepeatsLeft()
        {
            if (InternetChecker.RepeatsLeft() > 0)
            {
                textfield.text = "No internet connection. \n Please turn on the internet or wait ";
            }
            else
            {
                Helper.LoadNextScene();
            }
        }
    }
}