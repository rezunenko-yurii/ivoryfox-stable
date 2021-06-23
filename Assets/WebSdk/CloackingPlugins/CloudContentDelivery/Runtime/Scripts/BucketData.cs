using System;
using UnityEngine;

namespace CloudContentDelivery.Runtime.Scripts
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
