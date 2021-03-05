using System.Collections.Generic;
using UnityEngine;

namespace PluginsResolver.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackagesToDownload", menuName = "IvoryFox/Create/PackagesToDownload", order = 0)]
    public class PackagesToDownload : ScriptableObject
    {
        [SerializeField] public List<string> packages = new List<string>();
    }
}