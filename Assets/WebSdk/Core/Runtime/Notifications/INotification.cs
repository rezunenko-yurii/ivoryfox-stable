using System;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Notifications
{
    public interface INotification: IModule
    {
        event Action Prepared;
        bool IsUsing();
        bool IsPrepared();
    }
}