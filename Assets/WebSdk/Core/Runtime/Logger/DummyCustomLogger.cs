using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Logger
{
    public class DummyCustomLogger : MonoBehaviour, ICustomLogger
    {
        public bool IsReady { get; set; }
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

        public IModulesHost Parent { get; set; }
    }
}
