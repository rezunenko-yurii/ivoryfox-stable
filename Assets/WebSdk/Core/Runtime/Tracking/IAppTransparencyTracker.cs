using System;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public interface IAppTransparencyTracker : IModule
    {
        event Action RequestShowed;
        AttStatus Status{ get; }
        bool IsReady{ get; }
        void DoRequest();
    }
}
