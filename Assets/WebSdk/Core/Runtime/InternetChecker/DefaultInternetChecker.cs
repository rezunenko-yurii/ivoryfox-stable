using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public class DefaultInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> Checked;
        public event Action<bool> RepeatsEnded;

        private readonly int _infinityChecking = -1;
        
        //private 
        
        public bool HasConnection { get; private set; }
        private int _repeatCount;
        
        private IEnumerator SendRequest()
        {
            Debug.Log($"{nameof(DefaultInternetChecker)} {nameof(SendRequest)}");

            

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                HasConnection = false;
            }
            else
            {
                var request = new UnityWebRequest("http://google.com") {timeout = 5};
                yield return request.SendWebRequest();
                
                HasConnection = request.error == null;
                
                request.Dispose();
            }
            
            Checked?.Invoke(HasConnection);
            
            if (_repeatCount != _infinityChecking)
            {
                DecreaseRepeatCount();
            
                if (HasConnection || _repeatCount == 0)
                {
                    CancelInvoke(nameof(StartChecking));

                    Debug.Log("DefaultInternetChecker RepeatCount == 0");
                
                    RepeatsEnded?.Invoke(HasConnection);
                
                    RepeatsEnded = null;
                    Checked = null;
                } 
            }
            
        }

        private void DecreaseRepeatCount()
        {
            if (_repeatCount <= 0)
            {
                _repeatCount = 1;
            }

            _repeatCount--;
        }

        public void Check(int repeatCount = 1)
        {
            if (repeatCount > 1 || repeatCount == _infinityChecking)
            {
                _repeatCount = repeatCount;
                InvokeRepeating(nameof(StartChecking), 0f, 3f);
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