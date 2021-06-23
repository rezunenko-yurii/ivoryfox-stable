using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.Tracking;
using Debug = UnityEngine.Debug;

namespace WebSdk.Main.Runtime.Scripts
{
    public class WebSdkEntry : MonoBehaviour, IGlobalBlock
    {
        [SerializeField] private GameObject globalGameObject;
        [SerializeField] private GameObject webGameObject;
        [SerializeField] private GameObject trackingGameObject;
        
        public TextMeshProUGUI textfield;
        
        [SerializeField] private WebManager _webManager;
        [SerializeField] private TrackingManager _trackingManager;
        private Stopwatch _stopwatch;

        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");
            
            _trackingManager.Init(trackingGameObject);
            _webManager.Init(webGameObject);

            ModulesNavigation.SetWebBlockSettings();
            
            _stopwatch = Stopwatch.StartNew();

            GlobalFacade.Init(globalGameObject);
            CheckAtt();
        }
        
        private void CheckAtt()
        {
            Debug.Log("WebSdkEntry CheckAtt");
            
            _trackingManager.Att.OnGetRequest += CheckInternetConnection;
            _trackingManager.Att.DoRequest();
        }

        private void CheckInternetConnection()
        {
            Debug.Log("WebSdkEntry CheckInternetConnection");
            
            GlobalFacade.InternetChecker.OnResult += TryLoadConfigs;
            GlobalFacade.InternetChecker.Check(3);
        }

        #region Cofigs
        
        public void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"WebSdkEntry TryLoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection)
            {
                GlobalFacade.InternetChecker.OnResult -= TryLoadConfigs;
                LoadConfigs();
            }
            else
            {
                textfield.text = "No internet connection. \n Please turn on the internet or wait ";
                CheckRepeatsLeft();
            }
        }

        private void LoadConfigs()
        {
            Debug.Log($"WebSdkEntry LoadConfigs");
            var ids = ConfigLoaderHelper.GetConsumableIds(GlobalFacade.Logger, GlobalFacade.Notification);
            ids.Add("canUse");
            ids = ids.Union(_trackingManager.GetConfigIds()).ToList();
            ids = ids.Union(_webManager.GetConfigIds()).ToList();

            if (ids.Count > 0)
            {
                GlobalFacade.ConfigsLoader.Load(ids, InitConfigs);
            }
            else
            {
                Debug.Log($"WebSdkEntry -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
        }
        
        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"WebSdkEntry InitConfigs / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");

            var a = _webManager.GetModulesForConfigs();
            a = a.Union(_trackingManager.GetModulesForConfigs()).ToList();

            ConfigLoaderHelper.SetConfigsToConsumables(configs, a.ToArray());

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);

            if (canUse)
            {
                ConfigLoaderHelper.SetConfigsToConsumables(configs, GlobalFacade.Logger, GlobalFacade.Notification);
                
                Debug.Log($"GlobalBlockUnity Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
                
                _webManager.DoWork();
            }
            else
            {
                Debug.Log($"WebSdkEntry / canUse = false -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
            
            _stopwatch.Stop();
        }
        
        #endregion

        private void CheckRepeatsLeft()
        {
            if (GlobalFacade.InternetChecker.RepeatsLeft() > 0)
            {
                textfield.text = "No internet connection. \n Please turn on the internet or wait ";
            }
            else
            {
                Debug.Log($"WebSdkEntry -> No internet connection -> GoToNativeBlock");
                ModulesNavigation.GoToNativeBlock();
            }
        }

        private void OnDestroy()
        {
            Debug.Log($"WebSdkEntry OnDestroy");
            GlobalFacade.Logger.Clear();
        }
    }
}