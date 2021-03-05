using System;

namespace PluginsResolver.Editor.Scripts
{
    [Serializable]
    public class LocalPackageData
    {
        public string packageName;
        public string pathToPackage;
        public string installedPackageLocation;
    }
}