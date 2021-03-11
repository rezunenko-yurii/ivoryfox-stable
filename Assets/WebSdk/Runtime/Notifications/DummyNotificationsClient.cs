using System;
using GlobalBlock.Interfaces.Notifications;

namespace GlobalBlock.Interfaces
{
    public class DummyNotificationsClient: INotification
    {
        public event Action OnReady;

        public bool IsUsing()
        {
            return false;
        }

        public bool IsReady()
        {
            return false;
        }
    }
}