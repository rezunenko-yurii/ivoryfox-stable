using System;
using System.Collections;
using UnityEngine;

namespace GlobalBlock.Interfaces
{
    public class DummyInternetChecker : IInternetChecker
    {
        public event Action<bool> OnResult;
        
        public void Check(int repeatCount = 1)
        {
            OnResult?.Invoke(false);
            OnResult = null;
        }

        public int RepeatsLeft()
        {
            return 0;
        }

        public bool HasConnection { get; } = false;
        public bool IsBlocked { get; }
    }
}