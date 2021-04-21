using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Notifications
{
    public abstract class NotificationBase
    {
        protected const string ChanelId = "ivoryfox_chanel";
        protected const string GroupId = "ivoryfox_group";
        protected const string ConfigJsonName = "notificationsConfig";
        protected const string IsNotificationInitedPrefs = "is_notification_inited";
        
        protected NotificationConfig config;
        protected bool isReady = false;

        protected NotificationBase()
        {
            Debug.Log($"In constructor of {this.GetType()}");
            SubscribeForNotifications();
        }
        
        protected void InitNotifications()
        {
            foreach (var model in config.Items)
            {
                if (!model.IsInited())
                {
                    Debug.Log($"Init notification {model.id}");

                    DateTime dt = NotificationsHelper.GetTime(model);
                    Schedule(model, dt, model.fireTimeType == NotificationConfig.DateType);
                }
                else Debug.Log($"Notification {model.id} is already inited");
            }
        }
        protected abstract void Schedule(NotificationModel model, DateTime fireTime, bool isDate = false);
        protected abstract void SubscribeForNotifications();
        protected abstract void UnSubscribeForNotifications();
        
        protected abstract T PrepareContent<T>(NotificationModel model);
    }
}