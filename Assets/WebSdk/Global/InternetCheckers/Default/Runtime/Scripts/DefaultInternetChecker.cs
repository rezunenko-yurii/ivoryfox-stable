using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.GlobalPart;
using WebSdk.Core.Runtime.InternetChecker;

namespace WebSdk.Global.InternetCheckers.Default.Runtime.Scripts
{
    public class DefaultInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> OnRepeatCheckResult;
        public event Action<bool> OnRepeatEndResult;
        public bool HasConnection { get; private set; } = false;
        public bool IsBlocked { get; private set; } = false;
        private int _repeatCount = 0;
        private IEnumerator SendRequest()
        {
            Debug.Log("DefaultInternetChecker SendRequest");
            
            if (_repeatCount <= 0) _repeatCount = 1;
            _repeatCount--;
            
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                HasConnection = false;
            }
            else
            {
                UnityWebRequest request = new UnityWebRequest("http://google.com") {timeout = 10};
                yield return request.SendWebRequest();
                HasConnection = request.error == null;
            }
            
            OnRepeatCheckResult?.Invoke(HasConnection);

            if (HasConnection || _repeatCount == 0)
            {
                CancelInvoke(nameof(StartChecking));
                IsBlocked = false;
                
                Debug.Log("DefaultInternetChecker RepeatCount == 0");
                
                OnRepeatEndResult?.Invoke(HasConnection);
                
                OnRepeatEndResult = null;
                OnRepeatCheckResult = null;
            }
        }
        
        public void Check(int repeatCount = 1)
        {
            IsBlocked = true;
            
            if (repeatCount > 1)
            {
                _repeatCount = repeatCount;
                InvokeRepeating(nameof(StartChecking), 0f, 11f);
            }
            else
            {
                StartCoroutine(SendRequest());
            }
            
        }
        private void StartChecking()
        {
            StartCoroutine(SendRequest());
        }
        
        public int RepeatsLeft()
        {
            return _repeatCount;
        }

        public IModulesHost Parent { get; set; }
    }
}