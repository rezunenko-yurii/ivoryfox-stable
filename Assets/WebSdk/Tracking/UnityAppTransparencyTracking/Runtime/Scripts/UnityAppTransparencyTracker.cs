#if UNITY_IOS
using Unity.Advertisement.IosSupport;
using UnityEngine.iOS;
#endif
using System;
using System.Collections;

using UnityEngine;
using WebSdk.Core.Runtime.Tracking;
using Debug = UnityEngine.Debug;

namespace WebSdk.Tracking.UnityAppTransparencyTracking.Runtime.Scripts
{
    public class UnityAppTransparencyTracker: MonoBehaviour, IAppTransparencyTracker
    {
        public event Action RequestShowed;
        public AttStatus Status { get; private set; }
        public bool IsReady { get; private set; }

        public void DoRequest()
        {
#if UNITY_IOS && !UNITY_EDITOR
            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version versionForCheck = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AppTransparencyTracker IOS version is {currentVersion}");
 
            if(currentVersion >= versionForCheck)
            {
                Debug.Log($"AppTransparencyTracker IOS version is >= 14.5 /// Need to ask for tracking");
                StartCoroutine(WatchAttStatus());
            }
            else
            {
                Debug.Log($"AppTransparencyTracker IOS version is lower than 14.5 /// skip request");
                Status = AttStatus.AUTHORIZED;
                
                OnRequestShowed();
            }
#else
            Debug.Log($"AppTransparencyTracker platform is not IOS /// skip request");
            Status = AttStatus.AUTHORIZED;
            
            OnRequestShowed();
#endif
        }
#if UNITY_IOS && !UNITY_EDITOR
        private IEnumerator WatchAttStatus()
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            
            while (!IsReady)
            {
                yield return new WaitForSeconds(0.1f);
                GetAttStatus();
            }
        }
#endif
        private void GetAttStatus()
        {
#if UNITY_IOS && !UNITY_EDITOR
            var s = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if(s != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Status = (AttStatus) Enum.Parse(typeof(AttStatus), s.ToString());
                Debug.Log($"---------------- AppTransparencyTracker response from user // status is {Status.ToString()} / {s.ToString()}");
            }
            else
            {
                return;
            }

            IsReady = true;
            Debug.Log($"----- AppTransparencyTracker complete // status = {Status}");
#endif
            OnRequestShowed();
        }

        private void OnRequestShowed()
        {
            Debug.Log($"----- AppTransparencyTracker complete // SendOnGetRequest");
            
            RequestShowed?.Invoke();
            RequestShowed = null;
        }
    }
}
