﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.Helpers;
using WebSdk.Core.Runtime.Logger;

namespace WebSdk.Loggers.Remote.Runtime.Scripts
{
    public class RemoteLogger : WebSdk.Core.Runtime.Logger.ILogger, IConfigConsumer
    {
        public static event Action OnReady;

        public bool IsReady { get; set; } = false;
        public LoggerData loggerData { get; set; }
        public int counter { get; set; }
        private List<Coroutine> _coroutines = new List<Coroutine>();

        public void SetConfig(string json)
        {
            if (!string.IsNullOrEmpty(json) && !json.Equals("{}"))
            {
                loggerData = JsonUtility.FromJson<LoggerData>(json);
                
                Application.logMessageReceived += Send;
            
                IsReady = true;
            
                string ms = $"Старт логерра " 
                            + $"&url = {loggerData.url}"
                            + $"&deviceName = {SystemInfo.deviceName}"
                            + $"&deviceModel = {SystemInfo.deviceModel}"
                            + $"&deviceType = {SystemInfo.deviceType}"
                            + $"&deviceUniqueIdentifier = {SystemInfo.deviceUniqueIdentifier}";
            
                Send(ms, string.Empty, LogType.Log);
            
                OnReady?.Invoke();
            }
        }
        public void Send(string condition, string stacktrace, LogType type = LogType.Log)
        {
            if(!IsReady) return;
            
            counter++;
                
            var dict = new Dictionary<string, string>()
            {
                {"message", condition},
                {"stacktrace", stacktrace},
                {"logType", type.ToString()},
                {"time", DateTime.Now.ToString()},
                {"Counter", counter.ToString()},
            };
            
            var coroutine = GlobalFacade.MonoBehaviour.StartCoroutine(SendGetRequest(dict));
            _coroutines.Add(coroutine);
        }

        public IEnumerator SendGetRequest(Dictionary<string,string> parameters)
        {
            string url = loggerData.url;
                
            if (parameters.Count > 0) url = WebHelper.AttachParameters(url, parameters);

            using (UnityWebRequest webRequest  = UnityWebRequest.Get(url))
            {
                webRequest.timeout = 3;
                webRequest.disposeDownloadHandlerOnDispose = true;
                webRequest.disposeUploadHandlerOnDispose = true;
                
                yield return webRequest.SendWebRequest();
            }
        }

        public void Clear()
        {
            foreach (Coroutine coroutine in _coroutines)
            {
                GlobalFacade.MonoBehaviour.StopCoroutine(coroutine);
            }
            
            _coroutines.Clear();
            
            OnReady = null;
            Application.logMessageReceived -= Send;
        }
        public string ConfigName { get; } = "logger";
    }
}