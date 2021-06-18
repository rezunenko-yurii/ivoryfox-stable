using System;
using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;

namespace WebSdk.Core.Runtime.AppTransparencyTrackers
{
    public class DummyAppTrackingTransparency : IAppTransparencyTracker
    {
        public event Action OnGetRequest;
        public AttStatus Status => AttStatus.AUTHORIZED;


        public DummyAppTrackingTransparency()
        {
            Debug.Log($"--------- !!!!!!!!!!!! --------- DummyAppTrackingTransparency");
        }
        public void Init()
        {
            Debug.Log("DummyAppTrackingTransparency Init");
            //throw new NotImplementedException();
        }
    }
}