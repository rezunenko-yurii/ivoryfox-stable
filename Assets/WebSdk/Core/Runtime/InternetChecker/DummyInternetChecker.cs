using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public class DummyInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> OnResult;
        
        public void Check(int repeatCount = 1)
        {
            Debug.Log("------------- DummyInternetChecker Check");
            
            HasConnection = Application.internetReachability != NetworkReachability.NotReachable;
            OnResult?.Invoke(HasConnection);
            OnResult = null;
        }

        public int RepeatsLeft()
        {
            return 0;
        }

        public bool HasConnection { get; private set; }
        public bool IsBlocked { get; }
    }
}