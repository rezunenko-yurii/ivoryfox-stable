using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.WebCore;
using WebSdk.Core.Runtime.WebCore.Parameters;
using WebSdk.Tracking.AdjustProvider.Runtime.Scripts;
using Debug = UnityEngine.Debug;

[assembly: AlwaysLinkAssembly]

namespace WebSdk.Parameters.AdjustParameters.Runtime.Scripts
{
    [Id("adjust")]
    public class AdjustParameter : WaitableParameter, IModuleRequest
    {
        [SerializeField] private string aliasText;
        
        private const string AdIdPref = "adid";
        private const string Organic = "organic";
        private readonly int _maxWaitTime = 4;
        private Stopwatch _stopwatch;

        private AdjustProvider _adjustProvider;
        
        public override void Init()
        {
            _stopwatch = Stopwatch.StartNew();

            Debug.Log($"AdjustParameter Init");

            Alias = aliasText;

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

            base.Init();
        }

        protected override IEnumerator WatchValue()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

            while (!IsPrepared())
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
            string v = _adjustProvider.GetAttribution(Alias);

            Debug.Log($"AdjustParameter attribution key={Alias} value={v}");
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
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(SetAdjustValue)}:: Set {Alias} = {v} / StopWatch = {_stopwatch.Elapsed.Seconds}");

            Value = v;
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

        public List<Type> GetRequiredModules()
        {
            return new List<Type>(){typeof(AdjustProvider)};
        }

        public void SetRequiredModules(List<IModule> modules)
        {
            _adjustProvider = (AdjustProvider) modules.First(x => x.GetType() == typeof(AdjustProvider));
        }
    }
}