using UnityEngine;

namespace GlobalBlock.Interfaces
{
    public class DummyLogger : ILogger
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
    }
}
