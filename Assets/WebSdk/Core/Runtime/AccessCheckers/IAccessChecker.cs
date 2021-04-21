using System;

namespace WebSdk.Core.Runtime.AccessCheckers
{
    public interface IAccessChecker
    {
        event Action<bool> OnCheckResult;
        void Check();
    }
}