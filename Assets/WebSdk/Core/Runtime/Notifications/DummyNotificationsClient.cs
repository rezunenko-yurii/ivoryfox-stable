using System;
using GlobalBlock.Interfaces.Notifications;
using WebSdk.Core.Runtime.Notifications;

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