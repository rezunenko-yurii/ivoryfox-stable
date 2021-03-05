using UnityEngine;

namespace CloudContentDelivery.Editor.Scripts
{
    [CreateAssetMenu(fileName = "CloudContentSettings", menuName = "IvoryFox/Create Cloud Content Settings", order = 0)]
    public class CloudContentSettings : ScriptableObject
    {
        public string version;
        public string apiKey;
        public string contentFolderPath;
        public BucketData lastBucketData;
    }
}