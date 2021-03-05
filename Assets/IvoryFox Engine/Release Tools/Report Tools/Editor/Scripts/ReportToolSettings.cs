using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    [CreateAssetMenu(fileName = "ReportToolSettings", menuName = "IvoryFox/Report Tool/Create Settings")]
    public class ReportToolSettings : ScriptableObject
    {
        public string version;
        public string apkSignerPath;
    }
}