using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Logger
{
    public interface ICustomLogger : IModule
    {
        bool IsPrepared { get; set; }
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