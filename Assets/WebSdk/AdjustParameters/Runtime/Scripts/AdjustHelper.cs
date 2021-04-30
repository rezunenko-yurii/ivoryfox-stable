using System;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.AdjustParameters.Runtime.Scripts
{
    public class AdjustHelper
    {
        public bool IsReady;
        private AdjustEnvironment GetEnvironment => AdjustEnvironment.Production;
        private static AdjustHelper _instance;
        
        public static AdjustHelper Instance => _instance ??= new AdjustHelper();

        private AdjustHelper()
        {
            Debug.Log($"In AdjustHelper constructor");
            GlobalFacade.configsLoader.Load(new List<string>(){"adjustToken"}, Init);
        }

        private void Init(Dictionary<string, string> data)
        {
            Application.deepLinkActivated += onDeepLinkActivated;

            string token = data["adjustToken"];
            Debug.Log($"AdjustHelper Init // Token {token}");
            
            if (string.IsNullOrEmpty(token))
            {
                Debug.Log("Error: Adjust Token is Empty ");
                return;
            }
            
            AdjustConfig config = new AdjustConfig(token, GetEnvironment, false);
            config.setLogLevel(AdjustLogLevel.Verbose);
            config.setLogDelegate(Debug.Log);

            config.setEventSuccessDelegate(EventSuccessCallback);
            config.setEventFailureDelegate(EventFailureCallback);
            config.setSessionFailureDelegate(SessionFailureCallback);
            config.setSessionSuccessDelegate(SessionSuccessCallback);
            config.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
            config.setAttributionChangedDelegate(AttributionChangedCallback);
            
            Adjust.start(config);
        }
        
        public void AttributionChangedCallback(AdjustAttribution obj)
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
        
        #region Event Handlers
        private void onDeepLinkActivated(string url)
        {
            Application.deepLinkActivated -= onDeepLinkActivated;
            
            Debug.Log("Unity Deeplink URL: " + url);
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