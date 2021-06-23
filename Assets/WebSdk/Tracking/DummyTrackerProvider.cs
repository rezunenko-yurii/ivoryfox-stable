using UnityEngine;
using WebSdk.Core.Runtime.AdjustHelpers;

namespace WebSdk.Tracking
{
    public class DummyTrackerProvider : MonoBehaviour, ITrackingProvider
    {
        public bool IsReady { get; } = true;
        public string GetAttribution(string request)
        {
            Debug.Log("------------- DummyTrackerProvider GetAttribution // will return empty string");
            return string.Empty;
        }
    }
}