using System;
using System.Collections.Generic;
using UnityEngine;
using WebSdk.Core.Runtime.Global;

namespace WebSdk.Core.Runtime.Notifications
{
    public class DummyNotificationsClient: MonoBehaviour, INotification
    {
        public event Action Prepared;

        public bool IsUsing()
        {
            return false;
        }

        public bool IsPrepared()
        {
            return false;
        }
    }
}