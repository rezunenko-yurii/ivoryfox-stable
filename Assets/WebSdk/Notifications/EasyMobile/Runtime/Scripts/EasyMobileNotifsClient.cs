using System;
using System.Globalization;
using UnityEngine;
using WebSdk.Core.Runtime.ConfigLoader;
using WebSdk.Core.Runtime.Global;
using WebSdk.Core.Runtime.Notifications; //using EasyMobile;
using NotificationRepeat = WebSdk.Core.Runtime.Notifications.NotificationRepeat;

/*using EasyNotifs = EasyMobile.Notifications;
using LocalNotification = EasyMobile.LocalNotification;
using NotificationRepeat = EasyMobile.NotificationRepeat;*/


namespace WebSdk.Notifications.EasyMobile.Runtime.Scripts
{
    public class EasyMobileNotifsClient: INotification, IConfigConsumer
    {
        private NotificationConfig config;
        private bool isReady = false;
        public event Action Prepared;
        
        public string ConfigName { get; } = "notificationsConfig";
        public bool IsUsing() => config != null;
        public bool IsPrepared() => isReady;
        
        public EasyMobileNotifsClient()
        {
            Debug.Log($"EasyMobileNotificationsClient constructor");

            /*EasyNotifs.LocalNotificationOpened += OnReceived;
            if (!EasyNotifs.IsInitialized()) EasyNotifs.Init();*/
        }
        /*private void OnReceived(LocalNotification delivered)
        {
            Debug.Log($"Received notification: {delivered.content.title} {delivered?.actionId}");
            EasyNotifs.LocalNotificationOpened -= OnReceived;

            isReady = true;
            
            OnReady?.Invoke();
            OnReady = null;
        }*/
        
        public void SetConfig(string json)
        {
            //Debug.Log($"In SetConfig of EasyMobileNotificationsClient / is Inited = {EasyNotifs.IsInitialized()}");
            this.config = JsonUtility.FromJson<NotificationConfig>(json);

            if(!IsUsing() || isReady)
            {
                Debug.Log($"Notification config is null / return");
                return;
            }
            
            foreach (var model in this.config.Items)
            {
                Debug.Log($"config contains notification {model.id} {model.IsInited()} {model.fireTimeType}");
            }

            /*foreach (var model in this.config.Items)
            {
                if (!model.IsInited())
                {
                    Debug.Log($"Init notification {model.id}");
                    
                    switch (model.fireTimeType)
                    {
                        case NotificationConfig.DelayType:
                        case NotificationConfig.TimeType:
                            Schedule(model, NotificationsHelper.GetTime(model.time, NotificationConfig.timeFormat));
                            break;
                        
                        case NotificationConfig.DateType:
                            Schedule(model, NotificationsHelper.GetTime(model.time, NotificationConfig.dateFormat), true);
                            break;
                        
                        default:
                            Debug.LogWarning($"Notification wrong fire type = {model.fireTimeType}");
                            return;
                    }
                }
                else Debug.Log($"Notification {model.id} is already inited");
            }*/
        }

        public void Schedule(NotificationModel model, DateTime fireTime, bool isDate = false)
        {
            /*var notification = PrepareContent(model);
            var repeatType = GetRepeatType(model);
            var dt = GetTime(model.time, format);

            var pushID = isDate ? EasyNotifs.ScheduleLocalNotification(dt, notification) : 
                EasyNotifs.ScheduleLocalNotification(dt.TimeOfDay, notification, repeatType);
            
            if (!pushID.Equals(string.Empty))
            {
                model.SetAsInited(pushID);
                Debug.Log($"Notification is scheduled {pushID} {model.fireTimeType} {model.time} {dt.TimeOfDay}");
            }
            else
            {
                Debug.Log($"Notification {model.id} has empty pushID");
            }*/
        }
        
        private DateTime GetTime(string time, string format) => DateTime.ParseExact(time, format, CultureInfo.CurrentCulture);

        private NotificationRepeat GetRepeatType(NotificationModel model)
        {
            Debug.Log($"Trying to get notification repeat type id {model.id} {model.repeatType}");
            
            if (string.IsNullOrEmpty(model.repeatType)) return NotificationRepeat.None;
            else return (NotificationRepeat) Enum.Parse(typeof(NotificationRepeat), model.repeatType);
        }

        /*NotificationContent PrepareContent(NotificationModel notificationModel)
        {
            NotificationLocal local = GetLocal(notificationModel.locals);
            var content = new NotificationContent
            {
                title = local.title, body = local.body //, categoryId = EM_NotificationsConstants.UserCategory_ivory_fox
            };
            
            return content;
        }*/

        private NotificationLocal GetLocal(NotificationLocal[] notificationModelLocals)
        {
            string country = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            
            if (country.Equals("ru") || country.Equals("ua") || country.Equals("by") || country.Equals("md"))
            {
                return notificationModelLocals[0];
            }
            else if (country.Equals("de"))
            {
                return notificationModelLocals[1];
            }
            else
            {
                return notificationModelLocals[2];
            }
        }

        public IModulesHost Parent { get; set; }
    }
}