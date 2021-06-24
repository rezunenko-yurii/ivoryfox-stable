using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public class DefaultInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> Checked;
        public event Action<bool> RepeatsEnded;
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
            
            Checked?.Invoke(HasConnection);

            if (HasConnection || _repeatCount == 0)
            {
                CancelInvoke(nameof(StartChecking));
                IsBlocked = false;
                
                Debug.Log("DefaultInternetChecker RepeatCount == 0");
                
                RepeatsEnded?.Invoke(HasConnection);
                
                RepeatsEnded = null;
                Checked = null;
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