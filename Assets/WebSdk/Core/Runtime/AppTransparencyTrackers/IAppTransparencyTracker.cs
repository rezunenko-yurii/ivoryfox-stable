using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.AppTransparencyTrackers
{
    public interface IAppTransparencyTracker
    {
        event Action OnGetRequest;
        AttStatus Status{ get; }
        void Init();
    }
}
