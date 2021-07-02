using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public class TrackingManager : MonoBehaviour, IModulesHandler
    {
        [SerializeField] private GameObject trackingGameObject;

        private IAppTransparencyTracker _att;
        private ITrackingProvider _provider;

        public event Action Completed;

        public void PrepareForWork()
        {
            Debug.Log($"{nameof(TrackingManager)} {nameof(PrepareForWork)}");

            _att = trackingGameObject.gameObject.GetComponent<IAppTransparencyTracker>() ?? trackingGameObject.gameObject.AddComponent<DummyAppTrackingTransparency>();
            _provider = trackingGameObject.gameObject.GetComponent<ITrackingProvider>() ?? trackingGameObject.gameObject.AddComponent<DummyTrackingProvider>();
        }

        public void ResolveDependencies(ModulesOwner owner)
        {
            owner.Add(_att, _provider);
            owner.SatisfyRequirements(_att, _provider);
        }

        public void DoWork()
        {
            _att.RequestShowed += Completed;
            _att.DoRequest();
        }

        /*public List<string> GetConfigIds()
        {
            return ConfigLoaderHelper.GetConsumableIds(_provider);
        }

        public List<IModule> GetModulesForConfigs()
        {
            return new List<IModule> {_provider};
        }*/
    }
}