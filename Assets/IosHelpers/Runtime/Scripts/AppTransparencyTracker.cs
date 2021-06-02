using System;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
#endif
using UnityEngine;

namespace IosHelpers.Runtime.Scripts
{
    public static class AppTransparencyTracker
    {
        public static void Init()
        {
#if UNITY_IOS
            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version versionForCheck = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AdjustHelper IOS version is {currentVersion}");
 
            if(currentVersion >= versionForCheck)
            {
                Debug.Log($"AdjustHelper IOS version is >= 14.5 /// Need to ask for tracking");
                GetAttStatus();
            }
#endif
        }
#if UNITY_IOS
        private static void GetAttStatus()
        {
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            Debug.Log($"---------------- AppTransparencyTracker current status is {status}");
            
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                Debug.Log($"AppTransparencyTracker start of request authorization tracking");
            }
        }
#endif
    }
}
