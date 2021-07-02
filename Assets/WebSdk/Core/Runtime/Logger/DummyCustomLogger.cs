using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Logger
{
    public class DummyCustomLogger : MonoBehaviour, ICustomLogger
    {
        public bool IsPrepared { get; set; }
        public LoggerData loggerData { get; set; }
        public int counter { get; set; }
        
        public void Send(string condition, string stacktrace = "", LogType type = LogType.Log)
        {
            //throw new System.NotImplementedException();
        }

        public void Clear()
        {
            //throw new System.NotImplementedException();
        }
    }
}
