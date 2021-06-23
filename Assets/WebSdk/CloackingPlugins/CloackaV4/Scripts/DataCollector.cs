using System;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CloackaV4.Scripts
{
    public class DataCollector : MonoBehaviour
    {
        public event Action OnDataCollected;
        public AnalyticsModel analyticsModel;
        
        private Stopwatch _stopwatch;
        private bool _canCheck = true;
        private Referrer _referrer;
        private AdjustHelper _adjustHelper;

        private void Awake()
        {
            _stopwatch = new Stopwatch();
            analyticsModel = new AnalyticsModel();
            _referrer = new Referrer();
            _adjustHelper = new AdjustHelper();
        }

        public void Init()
        {
            _adjustHelper.Init(analyticsModel.AdjustToken);
            analyticsModel.IsEmulator = false;
            analyticsModel.DeviceId = SystemInfo.deviceUniqueIdentifier;
            analyticsModel.Locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            analyticsModel.ScreenSize = Screen.width / Screen.dpi;
            analyticsModel.ApiVersion = Helper.GetSdkInt();
            analyticsModel.Brand = SystemInfo.deviceName;
            analyticsModel.Uptime = Helper.GetUpTime();
            analyticsModel.IsAccelerometerDead = false;
            analyticsModel.Model = SystemInfo.deviceModel;

            _stopwatch.Start();
        }
        
        private void Update()
        {
            analyticsModel.RefCode = _referrer.RefValue;
            analyticsModel.AdjustId = _adjustHelper.AdId;

            bool hasAdjustId = !string.IsNullOrEmpty(analyticsModel.AdjustId);
            bool hasRefCode = !string.IsNullOrEmpty(analyticsModel.RefCode);

            if (_canCheck)
            {
                if (hasAdjustId && hasRefCode)
                {
                    Debug.Log("Adjust and RefCode are collected");
                
                    _canCheck = false;
                    OnDataCollected?.Invoke();
                    return;
                }
                else if(_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds >= 10000)
                {
                    Debug.Log("Waiting is more than 10 seconds / used organic");
                
                    _stopwatch.Stop();
                    _canCheck = false;
                    analyticsModel.RefCode = "organic";
                    analyticsModel.AdjustId = "organic";
                
                    OnDataCollected?.Invoke();
                    return;
                }
            }
        }
    }
}