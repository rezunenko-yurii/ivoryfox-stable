using System;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public interface IInternetChecker : IModule
    {
        event Action<bool> OnRepeatCheckResult;
        event Action<bool> OnRepeatEndResult;
        void Check(int repeatCount = 1);
        int RepeatsLeft();
        bool HasConnection { get;}
        bool IsBlocked { get; }
    }
}