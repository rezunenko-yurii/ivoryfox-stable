using System.Collections.Generic;
using UnityEngine;

namespace RemoteConfigHelper.Scripts
{
    [CreateAssetMenu(fileName = "RemoteConfigAsset", menuName = "Create Remote Config Asset", order = 0)]
    public class RemoteConfigModel : ScriptableObject
    {
        public string environmentName;
        public List<ConfigItemModel> configItems = new List<ConfigItemModel>();
        public List<RuleModel> rules = new List<RuleModel>();
    }
}