using System;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.PlayerLoop;

namespace IosHelpers.Runtime.Scripts
{
    public static class AppTransparencyTracker
    {
        //public event Action sentTrackingAuthorizationRequest;

        public static void Init()
        {
#if UNITY_IOS
            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version versionForCheck = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AdjustHelper IOS version is {currentVersion}");
 
            if(currentVersion >= versionForCheck)
            {
                Debug.Log($"AdjustHelper IOS version is >= 14.5");
                //IsUsedAtt = true;

                GetAttStatus();
            }
#else
            //sentTrackingAuthorizationRequest?.Invoke();
#endif
        }
#if UNITY_IOS
        private static void GetAttStatus()
        {
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
                Debug.Log($"AdjustHelper RequestAuthorizationTracking");
                //sentTrackingAuthorizationRequest?.Invoke();
            }
        }
#endif
    }
}
