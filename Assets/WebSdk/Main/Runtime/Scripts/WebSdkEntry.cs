using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;
using Debug = UnityEngine.Debug;

namespace WebSdk.Main.Runtime.Scripts
{
    public class WebSdkEntry : MonoBehaviour, IGlobalBlock
    {
        public TextMeshProUGUI textfield;

        private IWebManager _webManager;
        private Stopwatch _stopwatch;
        private ModulesNavigation _navigation;

        [SerializeField] private GameObject web;

        private void Awake()
        {
            Debug.Log("GlobalBlockUnity Awake");

            _webManager = web.GetComponent<IWebManager>();

            _navigation = new ModulesNavigation();
            _navigation.SetWebBlockSettings();
            
            _stopwatch = Stopwatch.StartNew();

            InitModules();
            CheckAtt();
        }
        
        private void InitModules()
        {
            Debug.Log("GlobalBlockUnity InitModules");

            var factory = Resources.Load<ScriptableObject>("WebSdkComponentsFactory") as IGlobalFactory;
            factory.GameObject = gameObject;
            GlobalFacade.Init(factory, this);
        }
        private void CheckAtt()
        {
            Debug.Log("Trying to get ATT");
            GlobalFacade.Att.OnGetRequest += CheckInternetConnection;
            GlobalFacade.Att.Init();
        }

        private void CheckInternetConnection()
        {
            GlobalFacade.InternetChecker.OnResult += TryLoadConfigs;
            GlobalFacade.InternetChecker.Check(3);
        }

        #region Cofigs
        
        public void TryLoadConfigs(bool hasConnection)
        {
            Debug.Log($"GlobalBlockUnity LoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection)
            {
                GlobalFacade.InternetChecker.OnResult -= TryLoadConfigs;
                LoadConfigs();
            }
            else CheckRepeatsLeft();
        }

        private void LoadConfigs()
        {
            List<string> ids = ConfigLoaderHelper.GetConsumableIds(GlobalFacade.Logger, GlobalFacade.Notification, GlobalFacade.AdjustHelper);
            ids.Add("canUse");

            if (ids.Count > 0)
            {
                GlobalFacade.ConfigsLoader.Load(ids, InitConfigs);
            }
            else
            {
                _navigation.GoToNativeBlock();
            }
        }
        
        public void InitConfigs(Dictionary<string, string> configs)
        {
            Debug.Log($"GlobalBlockUnity InitConfigs / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
            
            ConfigLoaderHelper.SetConfigsToConsumables(configs, GlobalFacade.AdjustHelper);

            configs.TryGetValue("canUse", out var canUseString);
            bool.TryParse(canUseString, out var canUse);

            if (canUse)
            {
                ConfigLoaderHelper.SetConfigsToConsumables(configs, GlobalFacade.Logger, GlobalFacade.Notification);
                
                Debug.Log($"GlobalBlockUnity Complete / StopWatch = {_stopwatch.Elapsed.Seconds} FromStart = {Time.realtimeSinceStartup}");
                
                _webManager.Init();
            }
            else
            {
                Debug.Log($"GlobalBlockUnity InitConfigs / canUse = false");
                _navigation.GoToNativeBlock();
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
                _navigation.GoToNativeBlock();
            }
        }

        private void OnDestroy()
        {
            GlobalFacade.Logger.Clear();
        }
    }
}