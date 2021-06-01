using System;
using System.Collections;
using System.Diagnostics;
using com.adjust.sdk;
using UnityEngine;
using UnityEngine.Scripting;
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
        private const string Organic = "organic";
        private readonly int _waitTime = 8;
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

            /*if (GlobalFacade.adjustHelper.IsUsedAtt)
            {
                if (GlobalFacade.adjustHelper.AttStatus < 3)
                {
                    Debug.Log($"AdjustParameter // ATT status = {GlobalFacade.adjustHelper.AttStatus} set organic");
                    SetAdjustValue(Organic);
                }
            }*/

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
                string adid = Adjust.getAdid();
                string idfa = Adjust.getIdfa();

                if (!string.IsNullOrEmpty(adid) || !string.IsNullOrEmpty(idfa))
                {
                    Debug.Log($"Adjust TryToGetDataFromAdjust: Waiting is over // Adjust.adid = {adid} // Adjust.idfa = {idfa}");
                    string value = !string.IsNullOrEmpty(adid) ? adid : idfa;
                    SetAdjustValue(value);
                }
                else
                {
                    Debug.Log("Adjust TryToGetDataFromAdjust: Waiting is over // add organic");
                    SetAdjustValue(Organic);
                }
            }
        }
        
        private void OnAdjustReady()
        {
            Debug.Log($"On Adjust Instance is Ready // trying to get attribution");
            string v = GlobalFacade.adjustHelper.GetAttribution(parameterAlias);

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