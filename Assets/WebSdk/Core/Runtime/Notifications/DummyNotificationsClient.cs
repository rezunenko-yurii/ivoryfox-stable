using System;
using UnityEngine;

namespace WebSdk.Core.Runtime.Notifications
{
    public class DummyNotificationsClient: MonoBehaviour, INotification
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