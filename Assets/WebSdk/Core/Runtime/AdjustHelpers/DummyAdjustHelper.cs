using UnityEngine;

namespace WebSdk.Core.Runtime.AdjustHelpers
{
    public class DummyAdjustHelper : IAdjustHelper
    {
        public bool IsReady => false;
        public bool IsUsedAtt => false;
        public int AttStatus => 0;

        public string GetAttribution(string request)
        {
            Debug.Log($"{nameof(DummyAdjustHelper)} GetAttribution always return organic");
            return "organic";
        }
    }
}