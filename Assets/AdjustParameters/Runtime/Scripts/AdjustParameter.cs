using System.Collections;
using System.Diagnostics;
using com.ivoryfox.websdk.parameters.Runtime;
using UnityEngine;
using WebBlock.ivoryfox.websdk.parameters.Runtime;
using Debug = UnityEngine.Debug;

namespace AdjustParameters.Runtime.Scripts
{
    [Id("adjust")]
    public class AdjustParameter : WaitableParameter
    {
        private const string Organic = "organic";
        private Stopwatch _stopwatch;

        public override void Init(MonoBehaviour monoBehaviour)
        {
            _stopwatch = Stopwatch.StartNew();
            
            if (AdjustHelper.Instance.isReady) OnAdjustReady();
            base.Init(monoBehaviour);
        }
        private void OnAdjustReady()
        {
            Debug.Log($"Adjust Ready // trying to get attribution");
            string v = AdjustHelper.Instance.GetAttribution(parameterAlias);

            Debug.Log($"On Adjust Ready key={parameterAlias} value={v}");
            SetAdjustValue(string.IsNullOrEmpty(v) ? "organic" : v);
        }

        protected override IEnumerator WatchValue()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            while (!IsReady())
            {
                yield return new WaitForSeconds(0.5f);

                CheckForOrganic();
            }

#elif UNITY_EDITOR
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(WatchValue)}:: для UNITY_EDITOR используем organic");
            _stopwatch.Stop();
            
            Value = Organic;
            yield return null;
#endif
            onReady?.Invoke(this);
        }

        private void CheckForOrganic()
        {
            if (Time.time - TimeFromInit > WaitTime)
            {
                Debug.Log("Waiting is over // add organic");
                SetAdjustValue(Organic);
            }
            else
            {
                if (AdjustHelper.Instance.isReady)
                {
                    OnAdjustReady();
                }
            }
        }
        private void SetAdjustValue(string v)
        {
            Debug.Log($"::{nameof(AdjustParameter)}.{nameof(CheckForOrganic)}:: Set adid = {v} / StopWatch = {_stopwatch.Elapsed.Seconds}");
            SetValue(v);
            _stopwatch.Stop();
        }
    }
}