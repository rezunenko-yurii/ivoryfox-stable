using com.adjust.sdk;
using UnityEngine;

namespace CloackaV4.Scripts
{
    public class AdjustHelper
    {
        public string AdId;
        
        public void Init(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                AdId = "organic";
                return;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
            AdjustConfig config = new AdjustConfig(token, AdjustEnvironment.Production, false);
            config.setLogLevel(AdjustLogLevel.Verbose);
            config.setLogDelegate(msg => Debug.Log(msg));
            
            //config.setAttributionChangedDelegate(AttributionChangedCallback);
            
            Adjust.start(config);
#else
            Debug.Log("ADID IS ORGANIC");
            AdId = "organic";
#endif
        }

        /*private void AttributionChangedCallback(AdjustAttribution obj)
        {
            AdId = obj.adid;
            Debug.Log($"Adjust Attribution Changed: campaign = {obj.campaign} " +
                      $"adgroup = {obj.adgroup} " +
                      $"network = {obj.network} " +
                      $"clickLabel = {obj.clickLabel}" +
                      $"trackerName = {obj.trackerName} " +
                      $"trackerToken = {obj.trackerToken} ");
        }*/
    }
}