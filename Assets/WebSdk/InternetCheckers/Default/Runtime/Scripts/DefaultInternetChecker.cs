using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.InternetChecker;

namespace WebSdk.InternetCheckers.Default.Runtime.Scripts
{
    public class DefaultInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> OnResult;
        public bool HasConnection { get; private set; } = false;
        public bool IsBlocked { get; private set; } = false;
        private int _repeatCount = 0;
        private IEnumerator SendRequest()
        {
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
                
            //IsChecked = true;

            if (HasConnection || _repeatCount == 0)
            {
                CancelInvoke(nameof(StartChecking));
                IsBlocked = false;
            }
            
            OnResult?.Invoke(HasConnection);
            //OnResult = null;
        }
        
        public void Check(int repeatCount = 1)
        {
            IsBlocked = true;
            
            if (repeatCount > 1)
            {
                this._repeatCount = repeatCount;
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
    }
}