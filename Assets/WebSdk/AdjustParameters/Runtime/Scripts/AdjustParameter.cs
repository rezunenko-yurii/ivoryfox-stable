using System.Collections;
using System.Diagnostics;
using com.adjust.sdk;
using UnityEngine;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.Parameters.Runtime.Scripts;
using Debug = UnityEngine.Debug;

namespace WebSdk.AdjustParameters.Runtime.Scripts
{
    [Id("adjust")]
    public class AdjustParameter : WaitableParameter
    {
        private const string Organic = "organic";
        private readonly int _waitTime = 8;
        private Stopwatch _stopwatch;
        private MonoBehaviour _monoBehaviour;

        public override void Init(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
            
            Debug.Log($"AdjustParameter Init");
            Debug.Log($"AdjustHelper is ready {AdjustHelper.Instance.IsReady}");
            
#if UNITY_IOS && !UNITY_EDITOR
            Version currentVersion = new Version(Device.systemVersion); // Parse the version of the current OS
            Version ios14_5 = new Version("14.5"); // Parse the iOS 13.0 version constant
            Debug.Log($"AdjustHelper IOS version is {currentVersion}");
 
            if(currentVersion >= ios14_5)
            {
                Debug.Log($"AdjustHelper IOS version is >= 14.5");
                InitForIos();
            }
            else
            {
                Start();
            }
            
#else
            Start();
#endif
        }

        private void Start()
        {
            _stopwatch = Stopwatch.StartNew();
            base.Init(_monoBehaviour);
        }

        private void InitForIos()
        {
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(Init)}:: Request IOS 14 Tracking status");
            
            Adjust.requestTrackingAuthorizationWithCompletionHandler((status) =>
            {
                Debug.Log($"::{nameof(AdjustParameter)}.{nameof(Init)}:: Request status is {status}");
                if (status == 0 || status == 1)
                {
                    SetAdjustValue(Organic);
                }
                
                Start();
            });
        }
        
        protected override IEnumerator WatchValue()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

            while (!IsReady())
            {
                yield return new WaitForSeconds(0.1f);

                TryToGetDataFromAdjust();
            }

#elif UNITY_EDITOR
            
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(WatchValue)}:: для UNITY_EDITOR используем organic");
            SetAdjustValue(Organic);
            
            yield return null;
            
#endif
        }

        private void TryToGetDataFromAdjust()
        {
            if (AdjustHelper.Instance.IsReady)
            {
                Debug.Log($"Adjust TryToGetDataFromAdjust: Adjust is Ready");
                OnAdjustReady();
            }
            else if (Time.time - TimeFromInit > _waitTime)
            {
                Debug.Log("Adjust TryToGetDataFromAdjust: Waiting is over // add organic");
                Debug.Log($"Adjust TryToGetDataFromAdjust: Adjust.adid = {Adjust.getAdid()}");
                
                SetAdjustValue(Organic);
            }
        }
        
        private void OnAdjustReady()
        {
            Debug.Log($"On Adjust Instance is Ready // trying to get attribution");
            string v = AdjustHelper.Instance.GetAttribution(parameterAlias);

            Debug.Log($"Adjust attribution key={parameterAlias} value={v}");
            SetAdjustValue(string.IsNullOrEmpty(v) ? "organic" : v);
        }
        
        private void SetAdjustValue(string v)
        {
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(SetAdjustValue)}:: Set {parameterAlias} = {v} / StopWatch = {_stopwatch.Elapsed.Seconds}");
            
            SetValue(v);
            _stopwatch.Stop();
        }
    }
}