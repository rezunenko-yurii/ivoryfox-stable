using System.Collections.Generic;
using UnityEngine;

namespace RemoteConfigHelper.Scripts
{
    [System.Serializable]
    public class RuleModel
    {
        public string name;
        public bool enabled;
        public string conditions;
        [Range(0,100)] public int rolloutPercentage = 100;
        public List<ConfigItemModel> configItems = new List<ConfigItemModel>();
    }
}