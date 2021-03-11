using Unity.Notifications;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace GlobalBlock.Notifications.UnityAndroidNotifications.Editor.Scripts
{
    public class NotificationsPreprocessBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        public void OnPreprocessBuild(BuildReport report)
        {
            NotificationSettings.AndroidSettings.RescheduleOnDeviceRestart = true;
        }
    }
}