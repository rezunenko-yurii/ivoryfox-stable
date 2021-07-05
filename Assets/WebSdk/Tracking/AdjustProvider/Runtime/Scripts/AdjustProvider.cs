using System;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Tracking;

namespace WebSdk.Tracking.AdjustProvider.Runtime.Scripts
{
    public class AdjustProvider : MonoBehaviour, ITrackingProvider, IConfigConsumer
    {
        private const string AdjustTokenPrefs = "adjust_token";
        
        [SerializeField] private string inAppToken = string.Empty;
        public bool IsReady { get; private set; }
        private AdjustData _adjustData;
        
        public string ConfigName { get; } = "adjust";

        private void Awake()
        {
            var t = SavedToken;

            if (string.IsNullOrEmpty(t) && !string.IsNullOrEmpty(inAppToken))
            {
                SavedToken = t = inAppToken;
            }
            
            SetAdjustConfig(t);
        }

        private string SavedToken
        {
            get => PlayerPrefs.GetString(AdjustTokenPrefs, string.Empty);
            set => PlayerPrefs.SetString(AdjustTokenPrefs, value);
        }
        public void SetConfig(string json)
        {
            Debug.Log($"---- AdjustProvider SetConfig // Json {json}");
            
            if (!string.IsNullOrEmpty(json) && !json.Equals("{}"))
            {
                _adjustData = JsonUtility.FromJson<AdjustData>(json);

                if (!string.IsNullOrEmpty(_adjustData.token) && !_adjustData.token.Equals(SavedToken))
                {
                    SavedToken = _adjustData.token;
                    SetAdjustConfig(_adjustData.token);
                }
            }
            else
            {
                Debug.Log($"!!!!!!!!!!!!!!!! AdjustHelper isn`t inited");
            }
        }

        private void SetAdjustConfig(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Debug.Log($"{nameof(AdjustProvider)} {nameof(SetAdjustConfig)} // token is empty");
                return;
            }
            
            Debug.Log($"{nameof(AdjustProvider)} {nameof(SetAdjustConfig)}  // token {token}");
            
            var config = new AdjustConfig(token, AdjustEnvironment.Production, false);
            config.setLogLevel(AdjustLogLevel.Verbose);
            config.setSendInBackground(true);
            config.setAttributionChangedDelegate(AttributionChangedCallback);
            config.setLaunchDeferredDeeplink(true);
                
            config.setAllowIdfaReading(true);
            config.setAllowiAdInfoReading(true);
            config.setAllowAdServicesInfoReading(true);
            config.setEventBufferingEnabled(true);
                
            Adjust.setEnabled(true);
            Adjust.start(config);
        }

        private void AttributionChangedCallback(AdjustAttribution obj)
        {
            Debug.Log("----------- Adjust AttributionChangedCallback Received!");
            Debug.Log($"---------- Adjust Attribution Changed: " +
                      $"adid = {obj?.adid} " +
                      $"campaign = {obj?.campaign} " +
                      $"adgroup = {obj?.adgroup} " +
                      $"network = {obj?.network} " +
                      $"clickLabel = {obj?.clickLabel}" +
                      $"trackerName = {obj?.trackerName} " +
                      $"trackerToken = {obj?.trackerToken} ");

            IsReady = true;
        }

        public string GetAttribution(string request)
        {
            Debug.Log($"Adjust GetAttribution {request}");
            var attribution = Adjust.getAttribution();
            
            return request switch
            {
                "adid" => attribution.adid,
                "campaign" => attribution.campaign,
                "network" => attribution.network,
                "adgroup" => attribution.adgroup,
                "clickLabel" => attribution.clickLabel,
                "trackerName" => attribution.trackerName,
                "trackerToken" => attribution.trackerToken,
                _ => string.Empty
            };
        }

        public string GetAdid()
        {
            return Adjust.getAdid();
        }
    }
}