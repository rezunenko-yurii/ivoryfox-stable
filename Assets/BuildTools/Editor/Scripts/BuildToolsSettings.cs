using System;
using UnityEngine;

namespace BuildTools.Editor.Scripts
{
    [Serializable, CreateAssetMenu(fileName = "BuildToolsSettings", menuName = "IvoryFox/Build Tools/Create Settings")]
    public class BuildToolsSettings : ScriptableObject
    {
        public string version;
        public string buildFolderPath;
        public string apkSignerPath;
    }
}
