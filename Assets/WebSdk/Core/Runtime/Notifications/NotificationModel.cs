using System;

namespace WebSdk.Core.Runtime.Notifications
{
    [Serializable]
    public class NotificationModel
    {
        public string id;
        public NotificationLocal[] locals;
        public string fireTimeType;
        public string time;
        public string repeatType;
    }
}