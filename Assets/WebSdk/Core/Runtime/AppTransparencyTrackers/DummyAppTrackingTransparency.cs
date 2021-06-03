using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.AppTransparencyTrackers
{
    public class DummyAppTrackingTransparency : IAppTransparencyTracker
    {
        public event Action OnGetRequest;
        public AttStatus Status => AttStatus.AUTHORIZED;

        public void Init()
        {
            Debug.Log("DummyAppTrackingTransparency Init");
            //throw new NotImplementedException();
        }
    }
}