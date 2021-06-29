using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public class DummyAppTrackingTransparency : MonoBehaviour, IAppTransparencyTracker
    {
        public event Action RequestShowed;
        public AttStatus Status => AttStatus.AUTHORIZED;
        public bool IsReady { get; } = true;

        public void DoRequest()
        {
            Debug.Log($"----------- DummyAppTrackingTransparency Init // Status = {Status.ToString()}");
            
            RequestShowed?.Invoke();
            RequestShowed = null;
        }

        public ModulesHost Parent { get; set; }
    }
}