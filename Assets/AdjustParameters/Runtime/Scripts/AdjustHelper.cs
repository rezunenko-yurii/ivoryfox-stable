using System;
using System.Collections.Generic;
using com.adjust.sdk;
using GlobalBlock.Interfaces;
using UnityEngine;

namespace AdjustParameters.Runtime.Scripts
{
    public class AdjustHelper
    {
        public bool isReady;
        private AdjustEnvironment GetEnvironment => AdjustEnvironment.Production;
        private static AdjustHelper instance;
        
        public static AdjustHelper Instance => instance ??= new AdjustHelper();

        private AdjustHelper()
        {
            GlobalFacade.configsLoader.Load(new List<string>(){"adjustToken"}, Init);
        }

        private void Init(Dictionary<string, string> data)
        {
            Application.deepLinkActivated += onDeepLinkActivated;

            string token = data["adjustToken"];
            Console.WriteLine($"Adjust Token {token}");
            
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Error: Adjust Token is Empty ");
                return;
            }
            
            AdjustConfig config = new AdjustConfig(token, GetEnvironment, false);
            config.setLogLevel(AdjustLogLevel.Verbose);
            config.setLogDelegate(msg => Debug.Log(msg));

            config.setEventSuccessDelegate(EventSuccessCallback);
            config.setEventFailureDelegate(EventFailureCallback);
            config.setSessionFailureDelegate(SessionFailureCallback);
            config.setSessionSuccessDelegate(SessionSuccessCallback);
            config.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
            config.setAttributionChangedDelegate(AttributionChangedCallback);
            
            Adjust.start(config);
        }
        
        private void AttributionChangedCallback(AdjustAttribution obj)
        {
            Console.WriteLine($"Adjust Attribution Changed: campaign = {obj.campaign} " +
                              $"adgroup = {obj.adgroup} " +
                              $"network = {obj.network} " +
                              $"clickLabel = {obj.clickLabel}" +
                              $"trackerName = {obj.trackerName} " +
                              $"trackerToken = {obj.trackerToken} ");

            isReady = true;
        }

        public string GetAttribution(string request)
        {
            Console.WriteLine($"Adjust GetAttribution {request}");
            AdjustAttribution attribution = Adjust.getAttribution();
            
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
            Console.WriteLine("Unity Deeplink URL: " + url);
        }
        
        private void DeferredDeeplinkCallback(string deeplinkURL) 
        {
            Console.WriteLine("Adjust Deeplink URL: " + deeplinkURL);
        }
                
        private void EventSuccessCallback(AdjustEventSuccess data) 
        {
            Console.WriteLine($"Adjust Event Success Event message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void EventFailureCallback(AdjustEventFailure data)
        {
            Console.WriteLine($"Adjust Event Failure Event message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void SessionFailureCallback (AdjustSessionFailure data) 
        {
            Console.WriteLine($"Adjust Session Failure message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }
                
        private void SessionSuccessCallback (AdjustSessionSuccess data) 
        {
            Console.WriteLine($"Adjust Session Success message = {data.Message} jsonResponse = {data.GetJsonResponse()}");
        }

        #endregion
    }
}