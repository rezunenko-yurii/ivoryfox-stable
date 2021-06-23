using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CloackaV4.Scripts
{
    public class InternetChecker : MonoBehaviour
    {
        public event Action<bool> OnResult;
        public bool HasConnection { get; private set; } = false;
        private int _repeatCount = 0;
        private const string Url = "https://google.com";
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
                var request = new UnityWebRequest(Url) {timeout = 10};
                yield return request.SendWebRequest();
                HasConnection = request.error == null;
            }
            
            if (HasConnection || _repeatCount == 0)
            {
                CancelInvoke(nameof(StartChecking));
            }
            
            OnResult?.Invoke(HasConnection);
            //OnResult = null;
        }
        
        public void Check(int repeatCount = 1)
        {
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
    }
}