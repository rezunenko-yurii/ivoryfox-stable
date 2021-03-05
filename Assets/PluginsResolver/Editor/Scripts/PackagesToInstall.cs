using System.Collections.Generic;
using UnityEngine;

namespace PluginsResolver.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackagesToInstall", menuName = "IvoryFox/Create/PackagesToIntall", order = 0)]
    public class PackagesToInstall : ScriptableObject
    {
        [SerializeField] public List<LocalPackageData> packages = new List<LocalPackageData>();
    }
}
