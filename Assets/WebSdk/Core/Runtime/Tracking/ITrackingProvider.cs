using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Tracking
{
    public interface ITrackingProvider : IModule
    {
        bool IsReady { get; }
        string GetAttribution(string request);
        string GetAdid();
    }
}