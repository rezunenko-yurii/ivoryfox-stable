using System.Collections;
using UnityEngine;

namespace WebSdk.Parameters.Runtime.Scripts
{
    public class WaitableParameter: Parameter
    {
        protected float TimeFromInit { get; private set; }
        protected const int WaitTime = 12;
        
        public override void Init(MonoBehaviour monoBehaviour)
        {
            TimeFromInit = Time.time;
            monoBehaviour.StartCoroutine(WatchValue());
        }

        protected virtual IEnumerator WatchValue()
        {
            yield return null;
        }
    }
}