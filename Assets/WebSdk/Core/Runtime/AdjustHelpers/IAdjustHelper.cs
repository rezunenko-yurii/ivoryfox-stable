using System;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.AdjustHelpers
{
    public interface IAdjustHelper : IModule
    {
        bool IsReady { get; }
        bool IsUsedAtt { get; }
        int AttStatus { get; }
        string GetAttribution(string request);
    }
}