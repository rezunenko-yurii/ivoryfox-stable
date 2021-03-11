using System;

namespace GlobalBlock.Interfaces.Notifications
{
    [Serializable]
    public class NotificationLocal
    {
        public string title;
        public string body;
        public string local;
    }
}