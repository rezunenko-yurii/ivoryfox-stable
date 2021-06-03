using System;
using System.Collections;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.iOS;
using WebSdk.Core.Runtime.AppTransparencyTrackers;
using Debug = UnityEngine.Debug;

namespace WebSdk.AppTransparencyTrackers.Unity.Runtime.Scripts
{
    public class UnityAppTransparencyTracker: MonoBehaviour, IAppTransparencyTracker
    {
        public event Action OnGetRequest;
        public AttStatus Status { get; private set; }

        private bool _isReady = false;
        private float _timeFromInit;
        private const int WaitTime = 10;

        public void Init()
        {
#if UNITY_IOS
            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version versionForCheck = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AppTransparencyTracker IOS version is {currentVersion}");
 
            if(currentVersion >= versionForCheck)
            {
                Debug.Log($"AppTransparencyTracker IOS version is >= 14.5 /// Need to ask for tracking");
                
                _timeFromInit = Time.time;
                StartCoroutine(WatchAttStatus());
            }
            else
            {
                Debug.Log($"AppTransparencyTracker IOS version is lower than 14.5 /// skip request");
                Status = AttStatus.AUTHORIZED;
                
                SendOnGetRequest();
            }
#else
            Debug.Log($"AppTransparencyTracker platform is not IOS /// skip request");
            Status = AttStatus.AUTHORIZED;
            
            SendOnGetRequest();
#endif
        }
#if UNITY_IOS
        private IEnumerator WatchAttStatus()
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            
            while (!_isReady)
            {
                yield return new WaitForSeconds(0.1f);
                GetAttStatus();
            }
        }
#endif
        private void GetAttStatus()
        {
            var s = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if(s != ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                Status = (AttStatus) Enum.Parse(typeof(AttStatus), s.ToString());
                Debug.Log($"---------------- AppTransparencyTracker response from user // status is {Status.ToString()} / {s.ToString()}");
            }
            else if (Time.time - _timeFromInit > WaitTime)
            {
                Status = AttStatus.DENIED;
                Debug.Log($"!!!!!!!!!!!!!!!!! AppTransparencyTracker time out // user did not response // status is {Status.ToString()} / {s.ToString()}");
            }
            else
            {
                return;
            }

            _isReady = true;
            Debug.Log($"----- AppTransparencyTracker complete // status = {Status}");

            SendOnGetRequest();
        }

        private void SendOnGetRequest()
        {
            Debug.Log($"----- AppTransparencyTracker complete // SendOnGetRequest");
            
            OnGetRequest?.Invoke();
            OnGetRequest = null;
        }
    }
}
