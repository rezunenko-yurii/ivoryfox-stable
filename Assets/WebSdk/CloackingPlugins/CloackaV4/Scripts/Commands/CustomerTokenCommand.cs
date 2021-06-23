using System;
using System.Collections.Generic;
using UnityEngine;

namespace CloackaV4.Scripts.Commands
{
    public class CustomerTokenCommand: BaseCommand, ICommand
    {
        public event Action<ResponseModel> OnResult;
        
        private UnityUrlLoader urlLoader;
        private const string UserToken = "User_Token";
        private DataCollector dataCollector;
        public string ReadToken => PlayerPrefs.GetString(UserToken, string.Empty);
        private void WriteToken(string value) => PlayerPrefs.SetString(UserToken, value);

        public CustomerTokenCommand(UnityUrlLoader urlLoader, DataCollector dataCollector)
        {
            this.urlLoader = urlLoader;
            this.dataCollector = dataCollector;
        }
        
        public void DoCommand()
        {
            Debug.Log("In CustomerTokenCommand.DoCommand");
            string json = dataCollector.analyticsModel.ToJson();
            
            var headers = new Dictionary<string, string>
            {
                {"Accept", "application/json"},
                {"x-device-data", Helper.Encode(json)},
#if UNITY_EDITOR
                {"X-debug-ip", "95.25.223.57"},
#endif
            };
            var parameters = new Dictionary<string, string>
            {
                {"dart", "wader"},
                {"moscow", dataCollector.analyticsModel.RefCode},
                {"fresh", dataCollector.analyticsModel.AppToken},
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
            Debug.Log(obj);
        }

        protected override void OnCommandSuccess(string obj)
        {
            UnSubscribe();
            var response = JsonUtility.FromJson<ResponseModel>(obj);

            WriteToken(response.data);
            OnResult?.Invoke(response);
        }
    }
}