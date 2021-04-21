using System;
using WebSdk.Core.Runtime.GlobalPart;

namespace WebSdk.Core.Runtime.Notifications
{
    public interface INotification: IModule
    {
        event Action OnReady;
        bool IsUsing();
        bool IsReady();
    }
}