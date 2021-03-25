using System;

namespace WebSdk.Runtime.AccessCheckers
{
    public interface IAccessChecker
    {
        event Action<bool> OnCheckResult;
        void Check();
    }
}