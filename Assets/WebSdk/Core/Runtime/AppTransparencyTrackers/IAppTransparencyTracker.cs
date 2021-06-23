using System;
using UnityEngine;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.AppTransparencyTrackers
{
    public interface IAppTransparencyTracker : IModule
    {
        event Action OnGetRequest;
        AttStatus Status{ get; }
        bool IsReady{ get; }
        void DoRequest();
    }
}
