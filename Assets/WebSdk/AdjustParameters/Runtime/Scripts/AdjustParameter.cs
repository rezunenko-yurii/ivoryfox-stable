using System;
using System.Collections;
using System.Diagnostics;
using com.adjust.sdk;
using UnityEngine;
using UnityEngine.Scripting;
using WebSdk.Core.Runtime.AppTransparencyTrackers;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.Parameters.Runtime.Scripts;
using Debug = UnityEngine.Debug;

[assembly: AlwaysLinkAssembly]

namespace WebSdk.AdjustParameters.Runtime.Scripts
{
    [Id("adjust")]
    public class AdjustParameter : WaitableParameter
    {
        private const string AdidPref = "adid";
        private const string Organic = "organic";
        private readonly int _waitTime = 4;
        private Stopwatch _stopwatch;
        private MonoBehaviour _monoBehaviour;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            Debug.Log("AdjustParameter Initialize");
        }

        [Preserve]
        public AdjustParameter()
        {
            Debug.Log("AdjustParameter Constructor");
        }
        
        public override void Init(MonoBehaviour monoBehaviour)
        {
            _stopwatch = Stopwatch.StartNew();
            _monoBehaviour = monoBehaviour;
            
            Debug.Log($"AdjustParameter Init");

            string savedAdid = PlayerPrefs.GetString(AdidPref, string.Empty);
            if (CheckSavedAdid(savedAdid))
            {
                Debug.Log($"AdjustParameter // Found saved adid // {savedAdid}");
                SetAdjustValue(savedAdid);
            }
            else if (GlobalFacade.att.Status == AttStatus.DENIED)
            {
                Debug.Log($"AdjustParameter // ATT status = {GlobalFacade.att.Status} set organic");
                SetAdjustValue(Organic);
            }

            base.Init(_monoBehaviour);
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
            if (GlobalFacade.adjustHelper.IsReady)
            {
                Debug.Log($"Adjust TryToGetDataFromAdjust: Adjust is Ready");
                OnAdjustReady();
            }
            else if (Time.time - TimeFromInit > _waitTime)
            {
                string adId = Adjust.getAdid();
                
                if (!string.IsNullOrEmpty(adId))
                {
                    Debug.Log($"-------------- Adjust TryToGetDataFromAdjust: Waiting is over // Adjust.adid = {adId} // Adjust.idfa = {Adjust.getIdfa()}");
                    SetAdjustValue(adId);
                }
                else
                {
                    Debug.Log($"!!!!!!!!!!!!!! Adjust TryToGetDataFromAdjust: Waiting is over // add organic // Adjust.idfa = {Adjust.getIdfa()}");
                    SetAdjustValue(Organic);
                }
            }
        }
        
        private void OnAdjustReady()
        {
            Debug.Log($"On Adjust Instance is Ready // trying to get attribution");
            string v = GlobalFacade.adjustHelper.GetAttribution(parameterAlias);

            Debug.Log($"AdjustParameter attribution key={parameterAlias} value={v}");
            if (CheckSavedAdid(v))
            {
                Debug.Log($"AdjustParameter remember adid // {v}");
                PlayerPrefs.SetString(AdidPref, v);
                
                SetAdjustValue(v);
            }
            else
            {
                SetAdjustValue("organic");
            }
        }
        
        private void SetAdjustValue(string v)
        {
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(SetAdjustValue)}:: Set {parameterAlias} = {v} / StopWatch = {_stopwatch.Elapsed.Seconds}");

            SetValue(v);
            _stopwatch.Stop();
        }

        private bool CheckSavedAdid(string v)
        {
            if (!v.Equals(Organic) && !v.Equals(string.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}