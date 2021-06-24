using com.adjust.sdk;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Tracking;

namespace WebSdk.Tracking.AdjustProvider.Runtime.Scripts
{
    public class AdjustProvider : MonoBehaviour, ITrackingProvider, IConfigConsumer
    {
        public bool IsReady { get; private set; }
        private AdjustData _adjustData;

        public AdjustProvider()
        {
            Debug.Log($"In AdjustHelper constructor");
        }

        public string ConfigName { get; } = "adjust";

        public void SetConfig(string json)
        {
            Debug.Log($"---- AdjustHelper SetConfig // Json {json}");
            
            if (!string.IsNullOrEmpty(json) && !json.Equals("{}"))
            {
                _adjustData = JsonUtility.FromJson<AdjustData>(json);
                
                if (string.IsNullOrEmpty(_adjustData.token))
                {
                    Debug.Log("!!!!!!!!!!!!!!!! Error: Adjust Token is Empty ");
                    return;
                }
                else Debug.Log($"AdjustHelper Init // Token {_adjustData.token}");

                var config = new AdjustConfig(_adjustData.token, AdjustEnvironment.Production, false);
                config.setLogLevel(AdjustLogLevel.Verbose);
                config.setSendInBackground(true);
                config.setAttributionChangedDelegate(AttributionChangedCallback);
            
                config.setDefaultTracker(_adjustData.token);
                config.setAllowIdfaReading(true);
                config.setPreinstallTrackingEnabled(true);
                config.setAllowiAdInfoReading(true);
                config.setAllowAdServicesInfoReading(true);
                
                config.setEventBufferingEnabled(true);
                //config.setProcessName();

                Adjust.setEnabled(true);
                Adjust.start(config);
                
                Debug.Log($"Adjust started");
            }
            else
            {
                Debug.Log($"!!!!!!!!!!!!!!!! AdjustHelper isn`t inited");
            }
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

        public IModulesHost Parent { get; set; }
    }
}