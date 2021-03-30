using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    public class ManifestHelper
    {
        private readonly string _manifestPath;
        private Dictionary<string, object> _manifestPackages;
        private Dictionary<string, object> _dependencies;
    
        private static ManifestHelper _instance;
        public static ManifestHelper GetInstance() => _instance ??= new ManifestHelper();

        [MenuItem("IvoryFox/Reload ManifestHelper")]
        public static void Reload()
        {
            GetInstance().Read(true);
        }
        private ManifestHelper()
        {
            var rootFolderPath = Application.dataPath.Replace("/Assets", "");
            _manifestPath = $"{rootFolderPath}/Packages/manifest.json";
            
            Read();
        }

        public void Read(bool forcibly = false)
        {
            if (_dependencies is null || forcibly)
            {
                string file = File.ReadAllText(_manifestPath);
                _manifestPackages = Json.Deserialize(file) as Dictionary<string, object>;
                _dependencies = _manifestPackages?["dependencies"] as Dictionary<string,object>;
            }
        }

        public void Write()
        {
            string m = Json.Serialize(_manifestPackages);
            File.WriteAllText(_manifestPath, m);
        }

        public bool Contains(string id)
        {
            return _dependencies.ContainsKey(id);
        }

        public void Add(string id, string value)
        {
            if (!Contains(id))
            {
                _dependencies.Add(id, value);
            }
        }

        public void Remove(string id)
        {
            if (Contains(id))
            {
                _dependencies.Remove(id);
            }
        }
    }
}
