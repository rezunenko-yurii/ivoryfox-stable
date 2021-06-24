using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.AppTransparencyTrackers;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public class TrackingManager : MonoBehaviour, IModulesManager
    {
        public GameObject HostGameObject { get; private set; }
        public IAppTransparencyTracker Att { get; private set; }
        private ITrackingProvider _provider;

        public void InitModules(GameObject trackingGameObject, IModulesHost parent)
        {
            Debug.Log("TrackingManager Init");
            Parent = parent;
            HostGameObject = trackingGameObject;
            
            Att = trackingGameObject.gameObject.GetComponent<IAppTransparencyTracker>() ?? trackingGameObject.gameObject.AddComponent<DummyAppTrackingTransparency>();
            _provider = trackingGameObject.gameObject.GetComponent<ITrackingProvider>() ?? trackingGameObject.gameObject.AddComponent<DummyTrackingProvider>();
            
            Modules = Parent.Modules;
            
            Att.Parent = this;
            _provider.Parent = this;
            
            Modules.Add(Att.GetType(), Att);
            Modules.Add(_provider.GetType(), _provider);
        }

        public Dictionary<Type, IModule> Modules { get; set; }
        public IModulesHost Parent { get; set; }

        public List<string> GetConfigIds()
        {
            return ConfigLoaderHelper.GetConsumableIds(_provider);
        }

        public List<IModule> GetModulesForConfigs()
        {
            return new List<IModule> {_provider};
        }

        public IModule GetModule(Type moduleType)
        {
            return Parent.GetModule(moduleType);
        }

        public void AddModule(Type moduleType, IModule module)
        {
            Parent.AddModule(moduleType, module);
        }
    }
}