using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public class DummyInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> OnRepeatCheckResult;
        public event Action<bool> OnRepeatEndResult;

        public void Check(int repeatCount = 1)
        {
            Debug.Log("------------- DummyInternetChecker Check");
            
            HasConnection = Application.internetReachability != NetworkReachability.NotReachable;
            OnRepeatCheckResult?.Invoke(HasConnection);
            OnRepeatEndResult?.Invoke(HasConnection);

            OnRepeatEndResult = null;
            OnRepeatCheckResult = null;
        }

        public int RepeatsLeft()
        {
            return 0;
        }

        public bool HasConnection { get; private set; }
        public bool IsBlocked { get; }
        public IModulesHost Parent { get; set; }
    }
}