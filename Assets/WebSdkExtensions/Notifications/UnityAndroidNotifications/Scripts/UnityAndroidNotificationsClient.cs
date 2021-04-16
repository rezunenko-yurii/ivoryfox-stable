using System;
using System.Globalization;
using System.Linq;
using GlobalBlock.Interfaces;
using GlobalBlock.Interfaces.Notifications;
using Unity.Notifications.Android;
using UnityEngine;

namespace WebSdkExtensions.Notifications.UnityAndroidNotifications.Scripts
{
    public class UnityAndroidNotificationsClient : NotificationBase, INotification, IConfigConsumer
    {
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
        
        private void OnReceived(AndroidNotificationIntentData data)
        {
            UnSubscribeForNotifications();
            
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);

            isReady = true;
            
            OnReady?.Invoke();
            OnReady = null;
        }

        private static void SetChannel()
        {
            //Debug.Log("Set Notifications Channel");
            
            var chanel = AndroidNotificationCenter.GetNotificationChannel(ChanelId);
            if (chanel.Id is null)
            {
                var channel = new AndroidNotificationChannel()
                {
                    Id = ChanelId,
                    Name = "IvoryFox Channel",
                    Importance = Importance.High,
                    Description = "Generic notifications",
                    EnableLights = true,
                    EnableVibration = true,
                    CanShowBadge = true,
                    LockScreenVisibility = LockScreenVisibility.Public,
                    CanBypassDnd = true,
                };
                
                AndroidNotificationCenter.RegisterNotificationChannel(channel);
            }
        }
        
        
#region INotification realizations

        public event Action OnReady;
        public bool IsUsing() => config != null;
        public bool IsReady()
        {
            if (config is null)
            {
                //Debug.Log($"Notification is not ready = config is null");
                return false;
            }
            
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
            if (notificationIntentData != null)
            {
                var id = notificationIntentData.Id;
                var channel = notificationIntentData.Channel;
                var notification = notificationIntentData.Notification;
                //Debug.Log($"Checking Last notification id = {id} channel = {channel}");
                
                if (config.Items.Any(x => x.id.Equals(id.ToString())))
                {
                    AndroidNotificationCenter.CancelAllDisplayedNotifications();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //Debug.Log($"Cannot get last notification");
                
                return false;
            }
        }

#endregion
        
#region IConfigConsumer realizations
        
        public string ConfigName { get; } = ConfigJsonName;
        public void SetConfig(string json)
        {
            //Debug.Log($"In SetConfig of UnityAndroidNotificationsClient");
            config = JsonUtility.FromJson<NotificationConfig>(json);
            
            if(!IsUsing()) // || IsReady())
            {
                //Debug.Log($"Cannot set notification config isUsing = {IsUsing()}");// / isReady = {isReady}");
                return;
            }

            config.PrintConfig();
            SetChannel();
            InitNotifications();
        }
        
#endregion

#region NotificationBase realizations

        protected override void SubscribeForNotifications()
        {
            /*Debug.Log($"Subscribe For Notifications of {nameof(UnityAndroidNotificationsClient)}");
            AndroidNotificationCenter.OnNotificationReceived += OnReceived;*/
        }

        protected override void UnSubscribeForNotifications()
        {
            /*Debug.Log($"UnSubscribe For Notifications of {nameof(UnityAndroidNotificationsClient)}");
            AndroidNotificationCenter.OnNotificationReceived -= OnReceived;
            AndroidNotificationCenter.CancelAllDisplayedNotifications();*/
        }

        protected override T PrepareContent<T> (NotificationModel model)
        {
            NotificationLocal local = GetLocal(model.locals);
            NotificationRepeat nr = NotificationsHelper.GetRepeatType(model);
            var n = new AndroidNotification
            {
                Title = local.title, 
                Text = local.body, 
                ShowTimestamp = true, 
                Color = Color.red,
                ShouldAutoCancel = true,
                Group = GroupId,
                GroupSummary = true,
                GroupAlertBehaviour = GroupAlertBehaviours.GroupAlertAll
            };
            
            if (nr != NotificationRepeat.None)
            {
                n.RepeatInterval = nr.ToTimeSpanInterval();
            }
            
            return (T) (object) n;
        }
        protected override void Schedule(NotificationModel model, DateTime fireTime, bool isDate = false)
        {
            //Debug.Log($"Schedule Notification id = {model.id} / date = {fireTime} / isDate = {isDate}");
            
            var n = PrepareContent<AndroidNotification>(model);
            n.FireTime = fireTime;
            
            Debug.Log($"Prepared Notification: title = {n.Title} / " +
                      $"text = {n.Text} / " +
                      $"repeatInterval = {n.RepeatInterval} / " +
                      $"fireTime = {fireTime} / " +
                      $"id = {Int32.Parse(model.id)}");
            
            AndroidNotificationCenter.SendNotificationWithExplicitID(n, ChanelId, Int32.Parse(model.id));
            
            model.SetAsInited(model.id);
        }

#endregion

    }
}