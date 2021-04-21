using System;
using System.Globalization;
using UnityEngine;

namespace WebSdk.Core.Runtime.Notifications
{
    public static class NotificationsHelper
    {
        public const string timeFormat = "HH:mm:ss";
        public const string TimeSpanTimeFormat = @"h\:m\:ss";
        public const string dateFormat = "dd/MM/yyyy HH:mm:ss";
        
        public static double ToSecondInterval(this NotificationRepeat t)
        {
            TimeSpan ts = t.ToTimeSpanInterval();

            Debug.Log($"Set notification repeat / NotificationRepeat = {t} TotalSeconds = {ts.TotalSeconds}");
            return ts.TotalSeconds;
        }
        
        public static TimeSpan ToTimeSpanInterval(this NotificationRepeat t)
        {
            TimeSpan ts;
            
            switch (t)
            {
                case NotificationRepeat.EveryMinute:
                    ts = TimeSpan.FromMinutes(1);
                    break;
                
                case NotificationRepeat.EveryHour:
                    ts =  TimeSpan.FromHours(1);
                    break;
                
                case NotificationRepeat.EveryDay:
                    ts =  TimeSpan.FromDays(1);
                    break;
                
                case NotificationRepeat.EveryWeek:
                    ts =  TimeSpan.FromDays(7);
                    break;
                
                default:
                    ts =  TimeSpan.Zero;
                    break;
            }

            return ts;
        }

        public static NotificationRepeat FromExactSecondInterval(double interval)
        {          
            var timeSpan = TimeSpan.FromSeconds(interval);

            if (timeSpan.TotalDays == 7)
                return NotificationRepeat.EveryWeek;
            else if (timeSpan.TotalDays == 1)
                return NotificationRepeat.EveryDay;
            else if (timeSpan.TotalHours == 1)
                return NotificationRepeat.EveryHour;
            else if (timeSpan.TotalMinutes == 1)
                return NotificationRepeat.EveryMinute;
            else
                return NotificationRepeat.None;
        }

        public static void PrintConfig(this NotificationConfig c)
        {
            foreach (var model in c.Items)
            {
                Debug.Log($"config contains notification id = {model.id} / isInited = {model.IsInited()}/ fireType = {model.fireTimeType}");
            }
        }
        
        public static bool IsInited(this NotificationModel model) 
        {
            string pid = PlayerPrefs.GetString("push_" + model.id, String.Empty);
            return !pid.Equals(string.Empty);
        }
        
        public static void SetAsInited(this NotificationModel model, string pushId)
        {
            if (!pushId.Equals(string.Empty))
            {
                PlayerPrefs.SetString("push_" + model.id, pushId);
                Debug.Log($"Notification is scheduled {model.id} {model.fireTimeType} {model.time}");
            }
            else
            {
                Debug.Log($"Notification {pushId} has empty pushID");
            }
        }
        
        public static DateTime GetTime(string time, string format)
        { 
            DateTime dt = DateTime.ParseExact(time, format, CultureInfo.CurrentCulture);
            Debug.Log($"In Notification GetTime / time = {time} format = {format} result = {dt}");
            return dt;
        }
        public static NotificationRepeat GetRepeatType(NotificationModel model)
        {
            NotificationRepeat nr;
            
            if (string.IsNullOrEmpty(model.repeatType)) nr = NotificationRepeat.None;
            else nr = (NotificationRepeat) Enum.Parse(typeof(NotificationRepeat), model.repeatType);

            Debug.Log($"Notification (model id = {model.id} / repeatType = {model.repeatType}) parsed repeatType = {nr}");
            
            return nr;
        }

        public static DateTime GetTime(NotificationModel model)
        {
            DateTime dt;
            TimeSpan ts;
            
            switch (model.fireTimeType)
            {
                case NotificationConfig.DelayType:
                    ts = TimeSpan.ParseExact(model.time, TimeSpanTimeFormat, CultureInfo.CurrentCulture);
                    dt = DateTime.Now.AddSeconds(ts.TotalSeconds);
                    break;
                
                case NotificationConfig.TimeType:
                    ts = TimeSpan.ParseExact(model.time, TimeSpanTimeFormat, CultureInfo.CurrentCulture);
                    dt = DateTime.Today.AddSeconds(ts.TotalSeconds);
                    break;
                        
                case NotificationConfig.DateType:
                    dt = DateTime.ParseExact(model.time, dateFormat, CultureInfo.CurrentCulture);
                    break;
                        
                default:
                    throw new Exception($"Notification wrong fire type = {model.fireTimeType}");
            }

            return dt;
        }
    }
}