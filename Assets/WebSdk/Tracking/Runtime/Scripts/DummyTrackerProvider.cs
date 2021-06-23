using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Tracking.Runtime.Scripts
{
    public class DummyTrackerProvider : MonoBehaviour, ITrackingProvider
    {
        public bool IsReady { get; } = true;
        public string GetAttribution(string request)
        {
            Debug.Log("------------- DummyTrackerProvider GetAttribution // will return empty string");
            return string.Empty;
        }

        public string GetAdid()
        {
            return string.Empty;
        }

        public IModulesHost Parent { get; set; }
    }
}