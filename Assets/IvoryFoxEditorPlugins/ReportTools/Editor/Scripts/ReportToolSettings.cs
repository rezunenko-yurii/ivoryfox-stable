using UnityEngine;

namespace ReportTools.Editor.Scripts
{
    [CreateAssetMenu(fileName = "ReportToolSettings", menuName = "IvoryFox/Report Tool/Create Settings")]
    public class ReportToolSettings : ScriptableObject
    {
        public string version;
        public string apkSignerPath;
    }
}