using System;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.InternetChecker
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