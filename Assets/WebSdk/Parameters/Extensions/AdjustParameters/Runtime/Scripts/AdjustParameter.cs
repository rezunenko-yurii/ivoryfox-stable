using System.Collections;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.Scripting;
using Debug = UnityEngine.Debug;

using WebSdk.Core.Runtime.WebCore;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Parameters.Runtime.Scripts;
using WebSdk.Tracking.Extensions.AdjustHelper.Runtime.Scripts;

[assembly: AlwaysLinkAssembly]

namespace WebSdk.Parameters.Extensions.AdjustParameters.Runtime.Scripts
{
    [Id("adjust")]
    public class AdjustParameter : WaitableParameter
    {
        private const string AdIdPref = "adid";
        private const string Organic = "organic";
        private readonly int _maxWaitTime = 4;
        private Stopwatch _stopwatch;
        private MonoBehaviour _monoBehaviour;

        private AdjustProvider _adjustProvider;

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

            _adjustProvider = (AdjustProvider) Parent.GetModule(typeof(AdjustProvider));
            
            Debug.Log($"AdjustParameter Init");

            string savedAdid = PlayerPrefs.GetString(AdIdPref, string.Empty);
            if (CheckSavedAdid(savedAdid))
            {
                Debug.Log($"AdjustParameter // Found saved adid // {savedAdid}");
                SetAdjustValue(savedAdid);
            }
            /*else if (GlobalFacade.Att.Status == AttStatus.DENIED)
            {
                Debug.Log($"AdjustParameter // ATT status = {GlobalFacade.Att.Status} set organic");
                SetAdjustValue(Organic);
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
            if (_adjustProvider.IsReady)
            {
                Debug.Log($"Adjust TryToGetDataFromAdjust: Adjust is Ready");
                OnAdjustReady();
            }
            else if (!string.IsNullOrEmpty(_adjustProvider.GetAdid()))
            {
                string adId = _adjustProvider.GetAdid();
                
                Debug.Log($"-------------- Adjust TryToGetDataFromAdjust: GetAdid alone // Adjust.adid = {adId}");
                SaveAdid(adId);
                SetAdjustValue(adId);
            }
            else if (Time.time - TimeFromInit > _maxWaitTime)
            {
                Debug.Log($"!!!!!!!!!!!!!! Adjust TryToGetDataFromAdjust: Waiting is over // add organic");
                SetAdjustValue(Organic);
            }
        }
        
        private void OnAdjustReady()
        {
            Debug.Log($"On Adjust Instance is Ready // trying to get attribution");
            string v = _adjustProvider.GetAttribution(parameterAlias);

            Debug.Log($"AdjustParameter attribution key={parameterAlias} value={v}");
            SaveAdid(v);
        }

        private void SaveAdid(string v)
        {
            if (CheckSavedAdid(v))
            {
                Debug.Log($"AdjustParameter remember adid // {v}");
                PlayerPrefs.SetString(AdIdPref, v);
                
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

        public new IModulesHost Parent => base.Parent;
    }
}