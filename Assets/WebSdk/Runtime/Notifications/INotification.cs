using System;

namespace GlobalBlock.Interfaces.Notifications
{
    public interface INotification: IModule
    {
        event Action OnReady;
        bool IsUsing();
        bool IsReady();
    }
}