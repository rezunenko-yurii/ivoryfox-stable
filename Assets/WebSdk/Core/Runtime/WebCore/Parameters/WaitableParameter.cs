using System.Collections;
using UnityEngine;

namespace WebSdk.Core.Runtime.WebCore.Parameters
{
    public class WaitableParameter: Parameter
    {
        protected float TimeFromInit { get; private set; }
        protected const int WaitTime = 12;
        
        public override void Init()
        {
            TimeFromInit = Time.time;
            StartCoroutine(WatchValue());
        }

        protected virtual IEnumerator WatchValue()
        {
            yield return null;
        }
    }
}