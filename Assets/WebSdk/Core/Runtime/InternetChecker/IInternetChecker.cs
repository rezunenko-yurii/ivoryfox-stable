using System;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.InternetChecker
{
    public interface IInternetChecker : IModule
    {
        event Action<bool> Checked;
        event Action<bool> RepeatsEnded;
        void Check(int repeatCount = 1);
        int RepeatsLeft();
        bool HasConnection { get;}
    }
}