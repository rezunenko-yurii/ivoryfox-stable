using System;
using System.Collections.Generic;
using UnityEngine;

namespace CloackaV4.Scripts.Commands
{
    public class CheckCommand: BaseCommand, ICommand
    {
        public event Action<ResponseModel> OnResult;
        
        private UnityUrlLoader urlLoader;
        private DataCollector dataCollector;
        
        public CheckCommand(UnityUrlLoader urlLoader, DataCollector dataCollector)
        {
            this.urlLoader = urlLoader;
            this.dataCollector = dataCollector;
        }
        
        public void DoCommand()
        {
            Debug.Log(" In CheckCommand.DoCommand");

            var headers = new Dictionary<string, string>
            {
                {"Accept", "application/json"},
                {"If-None-Match", dataCollector.analyticsModel.AdjustId},
                {"If-Range", dataCollector.analyticsModel.RefCode},
                #if UNITY_EDITOR
                {"X-debug-ip", "95.25.223.57"},
                #endif
            };
            var parameters = new Dictionary<string, string>
            {
                {"cherry", "red"},
                {"morty", dataCollector.analyticsModel.RefCode},
                {"fire", dataCollector.analyticsModel.AppToken},
                {"glow", dataCollector.analyticsModel.CustomerToken},
            };
            string a = Helper.AttachParameters(dataCollector.analyticsModel.Url, parameters);
        
            Subscribe();
            urlLoader.Load(a, headers);
        }
        protected override void Subscribe()
        {
            urlLoader.OnSuccess += OnCommandSuccess;
            urlLoader.OnFailure += OnCommandFailed;
        }
    
        protected override void UnSubscribe()
        {
            urlLoader.OnSuccess -= OnCommandSuccess;
            urlLoader.OnFailure -= OnCommandFailed;
        }
        
        protected override void OnCommandFailed(string obj)
        {
            UnSubscribe();
            Debug.Log(" In Check Failed");
            Debug.Log(obj);
        }
        protected override void OnCommandSuccess(string obj)
        {
            UnSubscribe();
            Debug.Log($"In Check Success {obj}");

            var response = JsonUtility.FromJson<ResponseModel>(obj);
        
            OnResult?.Invoke(response);
        }
    }
}