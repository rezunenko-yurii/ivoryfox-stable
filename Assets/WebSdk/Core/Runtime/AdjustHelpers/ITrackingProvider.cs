using System;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.AdjustHelpers
{
    public interface ITrackingProvider : IModule
    {
        bool IsReady { get; }
        string GetAttribution(string request);
    }
}