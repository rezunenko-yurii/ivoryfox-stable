using System;
using UnityEngine;

namespace GlobalBlock.Interfaces
{
    public interface ILogger : IModule
    {
        bool IsReady { get; set; }
        LoggerData loggerData { get; set; }
        int counter { get; set; }
        void Send(string condition, string stacktrace = "", LogType type = LogType.Log);
        void Clear();
    }
    
    [Serializable]
    public class LoggerData
    {
        public string url;
    }
}