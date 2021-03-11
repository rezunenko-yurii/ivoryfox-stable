using System;
using UnityEngine;

namespace GlobalBlock.Interfaces.Notifications
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