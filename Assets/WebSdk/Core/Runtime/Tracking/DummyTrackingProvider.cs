using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public class DummyTrackingProvider : MonoBehaviour, ITrackingProvider
    {
        public bool IsReady => false;
        public bool IsUsedAtt => false;
        public int AttStatus => 0;

        public DummyTrackingProvider()
        {
            Debug.Log($"--------- !!!!!!!!!!!! --------- DummyAdjustHelper");
        }
        public string GetAttribution(string request)
        {
            Debug.Log($"---------- {nameof(DummyTrackingProvider)} GetAttribution always return organic");
            return "organic";
        }

        public string GetAdid()
        {
            return string.Empty;
        }
    }
}