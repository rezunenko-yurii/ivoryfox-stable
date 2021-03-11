using System;
using System.Collections;

namespace GlobalBlock.Interfaces
{
    public interface IInternetChecker : IModule
    {
        event Action<bool> OnResult;
        void Check(int repeatCount = 1);
        int RepeatsLeft();
        bool HasConnection { get;}
        bool IsBlocked { get; }
    }
}