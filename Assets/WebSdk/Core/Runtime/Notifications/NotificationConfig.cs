using System;
using System.Collections.Generic;
using UnityEngine;

namespace WebSdk.Core.Runtime.Notifications
{
    [Serializable]
    public class NotificationConfig
    {
        public const string DelayType = "delay";
        public const string TimeType = "time";
        public const string DateType = "date";

        [SerializeField]
        public List<NotificationModel> Items = new List<NotificationModel>();
    }
}