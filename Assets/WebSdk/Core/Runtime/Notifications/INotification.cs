using System;
using GlobalBlock.Interfaces;

namespace WebSdk.Core.Runtime.Notifications
{
    public interface INotification: IModule
    {
        event Action OnReady;
        bool IsUsing();
        bool IsReady();
    }
}