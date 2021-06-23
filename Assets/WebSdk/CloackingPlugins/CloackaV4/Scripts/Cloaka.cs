using System.Collections.Generic;
using System.Diagnostics;
using CloackaV4.Scripts.Commands;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Global.ConfigLoaders.UnityRemoteConfig.Runtime.Scripts;
using Debug = UnityEngine.Debug;

namespace CloackaV4.Scripts
{
    public class Cloaka : MonoBehaviour
    {
        private DataCollector _dataCollector;
        private UnityUrlLoader _urlLoader;
    
        private CustomerTokenCommand _customerTokenCommand;
        private CheckCommand _checkCommand;
        private RemoteConfigsLoader _remoteConfig;
        private InternetChecker _internetChecker;
    
        void Start()
        {
            _remoteConfig = new RemoteConfigsLoader();
            _urlLoader = GetComponent<UnityUrlLoader>();
            _dataCollector = GetComponent<DataCollector>();
            _internetChecker = GetComponent<InternetChecker>();
            
            _internetChecker.OnResult += CheckConnection;
            _internetChecker.Check(3);
        }

        private void CheckConnection(bool hasConnection)
        {
            Debug.Log($"GlobalBlockUnity LoadConfigs / hasConnection {hasConnection}");
            
            if (hasConnection)
            {
                _internetChecker.OnResult -= CheckConnection;
                _remoteConfig.Load("data", AnalyzeRemoteConfigData);
            }
            else
            {
                CheckRepeatsLeft();
            }
        }
        
        private void CheckRepeatsLeft()
        {
            if (_internetChecker.RepeatsLeft() > 0)
            {
                //textfield.text = "No internet connection. \n Please turn on the internet or wait ";
            }
            else
            {
                Helper.LoadNextScene();
            }
        }

        private void AnalyzeRemoteConfigData(Dictionary<string, string> obj)
        {
            Debug.Log("In AnalyzeRemoteConfigData");
            if (obj["data"].Length > 2)
            {
                var remoteData = JsonUtility.FromJson<RemoteData>(obj["data"]);
                _dataCollector.analyticsModel.AppToken = remoteData.appToken;
                _dataCollector.analyticsModel.AdjustToken = remoteData.adjustToken;
                _dataCollector.analyticsModel.Url = remoteData.url;
                
                _dataCollector.Init();
                _dataCollector.OnDataCollected += DoTokenCommand;
            }
            else
            {
                Debug.Log("remote config is empty");
                Helper.LoadNextScene();
            }
        }

        private void DoTokenCommand()
        {
            Debug.Log("In DoTokenCommand");
            _dataCollector.OnDataCollected -= DoCheckCommand;
        
            _customerTokenCommand = new CustomerTokenCommand(_urlLoader, _dataCollector);
        
            if(string.IsNullOrEmpty(_customerTokenCommand.ReadToken))
            {
                Debug.Log("Customer token is empty / Will try to get new one");
                _customerTokenCommand.OnResult += AnalyzeTokenCommandResult;
                _customerTokenCommand.DoCommand();
            }
            else
            {
                DoCheckCommand();
            }
        }

        private void AnalyzeTokenCommandResult(ResponseModel response)
        {
            Debug.Log($"In AnalyzeTokenCommandResult");
            
            if (response != null && response.status)
            {
                Debug.Log($"Got token {response.data}");
                DoCheckCommand();
            }
            else
            {
                Debug.Log("status = false");
                Helper.LoadNextScene();
            }
        }
        private void DoCheckCommand()
        {
            Debug.Log($"In DoCheckCommand");
            
            _dataCollector.analyticsModel.CustomerToken = _customerTokenCommand.ReadToken;
        
            _checkCommand = new CheckCommand(_urlLoader, _dataCollector);
            _checkCommand.OnResult += AnalyzeCheckCommandResult;
            _checkCommand.DoCommand();
        }

        private void AnalyzeCheckCommandResult(ResponseModel response)
        {
            Debug.Log($"In AnalyzeCheckCommandResult");
            
            if (response != null && response.status)
            {
                Debug.Log($"Got url {response.data}");

                DoFirstLaunchCommand();
            
                string url = UnityWebRequest.UnEscapeURL(response.data);
                OpenWebView(url);
            }
            else
            {
                Debug.Log("status = false");
                Helper.LoadNextScene();
            }
        }

        private void DoFirstLaunchCommand()
        {
            Debug.Log($"In DoFirstLaunchCommand");
            if (FirstLaunchCommand.IsFirstLaunch.Equals("false"))
            {
                Debug.Log($"IsFirstLaunch is false");
                
                var firstLaunch = new FirstLaunchCommand(_urlLoader, _dataCollector);
                firstLaunch.DoCommand();
            }
        }

        private void OpenWebView(string url)
        {
            if (Helper.IsValidUrl(url))
            {
                Debug.Log("open final url " + url);
            
#if UNITY_EDITOR
                Process.Start(url);
#else
            var webViewClient = GetComponent<UniWebViewClient>();
            webViewClient.SetSettings();
            webViewClient.Open(url);
#endif
            
            }
            else
            {
                Debug.Log("not valid url " + url);
                Helper.LoadNextScene();
            }
        }
    }
}
