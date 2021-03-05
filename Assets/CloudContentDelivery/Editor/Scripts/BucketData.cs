using System;
using System.Collections.Generic;
using CloudContentDeliveryManagment.Model;
using UnityEngine;

namespace CloudContentDelivery.Editor.Scripts
{
    [Serializable]
    [CreateAssetMenu(fileName = "BucketSettings", menuName = "IvoryFox/Create Bucket Settings", order = 0)]
    public class BucketData : ScriptableObject
    {
        public string bucketName;
        public string bucketId;
        public string lastReleaseId;
    }
}
