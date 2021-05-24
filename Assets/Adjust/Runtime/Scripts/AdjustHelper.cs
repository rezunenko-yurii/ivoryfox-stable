using System;
using com.adjust.sdk;
using UnityEngine;
using UnityEngine.iOS;
using WebSdk.Core.Runtime.AdjustHelpers;
using WebSdk.Core.Runtime.ConfigLoader;

namespace Adjust.Runtime.Scripts
{
    public class AdjustHelper : IAdjustHelper, IConfigConsumer
    {
        public bool IsReady { get; private set; }
        public bool IsUsedAtt { get; private set; }
        public int AttStatus { get; private set; }
        private AdjustEnvironment GetEnvironment => AdjustEnvironment.Production;
        private AdjustData _adjustData;

        public AdjustHelper()
        {
            Debug.Log($"In AdjustHelper constructor");
            
#if UNITY_IOS && !UNITY_EDITOR

            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version versionForCheck = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AdjustHelper IOS version is {currentVersion}");
 
            if(currentVersion >= versionForCheck)
            {
                Debug.Log($"AdjustHelper IOS version is >= 14.5");
                IsUsedAtt = true;

                GetAttStatus();
            }
#endif
        }
        
        private void GetAttStatus()
        {
            Debug.Log($"Adjust Request IOS 14 Tracking status");
            
            com.adjust.sdk.Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
            {
                Debug.Log($"Adjust ATT Request status is {status}");
                AttStatus = status;
            });
        }

        public string ConfigName { get; } = "adjust";
        public void SetConfig(string json)
        {
            Debug.Log($"---- AdjustHelper SetConfig // Json {json}");
            
            if (!string.IsNullOrEmpty(json) && !json.Equals("{}"))
            {
                _adjustData = JsonUtility.FromJson<AdjustData>(json);
                
                Debug.Log($"AdjustHelper Init // Token {_adjustData.token}");
            
                if (string.IsNullOrEmpty(_adjustData.token))
                {
                    Debug.Log("Error: Adjust Token is Empty ");
                    return;
                }
                
                AdjustConfig config = new AdjustConfig(_adjustData.token, GetEnvironment, false);
                config.setLogLevel(AdjustLogLevel.Verbose);
                config.setLogDelegate(Debug.Log);

                config.setEventSuccessDelegate(EventSuccessCallback);
                config.setEventFailureDelegate(EventFailureCallback);
                config.setSessionFailureDelegate(SessionFailureCallback);
                config.setSessionSuccessDelegate(SessionSuccessCallback);
                config.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
                config.setAttributionChangedDelegate(AttributionChangedCallback);
            
                com.adjust.sdk.Adjust.start(config);
            }
            else
            {
                Debug.Log($"---- AdjustHelper isn`t inited");
            }
        }
        
        public string GetAttribution(string request)
        {
            Debug.Log($"Adjust GetAttribution {request}");
            var attribution = com.adjust.sdk.Adjust.getAttribution();
            
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
        
        #region Event Handlers
        
        private void AttributionChangedCallback(AdjustAttribution obj)
        {
            Debug.Log("Adjust AttributionChangedCallback Received!");
            Debug.Log($"Adjust Attribution Changed: " +
                      $"adid = {obj?.adid} " +
                      $"campaign = {obj?.campaign} " +
                      $"adgroup = {obj?.adgroup} " +
                      $"network = {obj?.network} " +
                      $"clickLabel = {obj?.clickLabel}" +
                      $"trackerName = {obj?.trackerName} " +
                      $"trackerToken = {obj?.trackerToken} ");

            IsReady = true;
        }
        
        private void DeferredDeeplinkCallback(string deeplinkURL) 
        {
            Debug.Log("Adjust Deeplink URL: " + deeplinkURL);
        }
                
        private void EventSuccessCallback(AdjustEventSuccess data) 
        {
            Debug.Log($"Adjust Event Success Event message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void EventFailureCallback(AdjustEventFailure data)
        {
            Debug.Log($"Adjust Event Failure Event message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void SessionFailureCallback (AdjustSessionFailure data) 
        {
            Debug.Log($"Adjust Session Failure message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void SessionSuccessCallback (AdjustSessionSuccess data) 
        {
            Debug.Log($"Adjust Session Success message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }

        #endregion
    }
}