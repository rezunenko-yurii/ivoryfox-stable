using System;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

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
