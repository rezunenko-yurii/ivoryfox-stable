using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.AppTransparencyTrackers
{
    public class DummyAppTrackingTransparency : MonoBehaviour, IAppTransparencyTracker
    {
        public event Action OnGetRequest;
        public AttStatus Status => AttStatus.AUTHORIZED;
        public bool IsReady { get; } = true;

        public void DoRequest()
        {
            Debug.Log($"----------- DummyAppTrackingTransparency Init // Status = {Status.ToString()}");
            
            OnGetRequest?.Invoke();
            OnGetRequest = null;
        }
    }
}