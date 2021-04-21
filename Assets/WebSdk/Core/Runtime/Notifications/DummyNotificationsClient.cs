using System;

namespace WebSdk.Core.Runtime.Notifications
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