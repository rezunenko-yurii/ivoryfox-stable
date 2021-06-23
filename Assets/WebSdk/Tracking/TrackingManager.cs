using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;
using WebSdk.Core.Runtime.AppTransparencyTrackers;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Tracking
{
    public class TrackingManager : MonoBehaviour, IConfigsHandler
    {
        public IAppTransparencyTracker Att { get; private set; }
        private ITrackingProvider _provider;

        public void Init(GameObject trackingGameObject)
        {
            Debug.Log("TrackingManager Init");
            
            Att = trackingGameObject.gameObject.GetComponent<IAppTransparencyTracker>() ?? trackingGameObject.gameObject.AddComponent<DummyAppTrackingTransparency>();
            _provider = trackingGameObject.gameObject.GetComponent<ITrackingProvider>() ?? trackingGameObject.gameObject.AddComponent<DummyTrackingProvider>();
        }

        public List<string> GetConfigIds()
        {
            return ConfigLoaderHelper.GetConsumableIds(_provider);
        }

        public List<IModule> GetModulesForConfigs()
        {
            return new List<IModule> {_provider};
        }
    }
    
}