using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public class DummyInternetChecker : MonoBehaviour, IInternetChecker
    {
        public event Action<bool> Checked;
        public event Action<bool> RepeatsEnded;

        public void Check(int repeatCount = 1)
        {
            Debug.Log("------------- DummyInternetChecker Check");
            
            HasConnection = Application.internetReachability != NetworkReachability.NotReachable;
            Checked?.Invoke(HasConnection);
            RepeatsEnded?.Invoke(HasConnection);

            RepeatsEnded = null;
            Checked = null;
        }

        public int RepeatsLeft()
        {
            return 0;
        }

        public bool HasConnection { get; private set; }
    }
}