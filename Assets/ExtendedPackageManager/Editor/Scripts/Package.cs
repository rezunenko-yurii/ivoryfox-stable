using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtendedPackageManager.Editor.Scripts
{
    [CreateAssetMenu(fileName = "PackageModel", menuName = "IvoryFox/Create/PackageModel", order = 0)]
    public class Package : ScriptableObject
    {
        public string packageId;
        public PackageTypes type;
        public string url;
        public List<string> unityDependencies = new List<string>();
        public List<string> gitDependencies = new List<string>();

        public void Install()
        {
            var helper = ManifestHelper.GetInstance();
            helper.Read();

            if (!helper.Contains(packageId))
            {
                helper.Add(packageId, url);
                WriteAndUpdate();

                CheckDependencies();
            }
        }

        public void Remove()
        {
            var helper = ManifestHelper.GetInstance();

            helper.Remove(packageId);
            WriteAndUpdate();
        }

        private void WriteAndUpdate()
        {
            ManifestHelper.GetInstance().Write();
        }

        private void CheckDependencies()
        {
            LoadGitDependencies();
            LoadUnityDependecies();
        }

        private void LoadUnityDependecies()
        {
            UnityRegistryHelper.Download(unityDependencies);
        }

        private void LoadGitDependencies()
        {
            var helper = ManifestHelper.GetInstance();
            var all = Resources.LoadAll<Package>("");
            
            foreach (string dependency in gitDependencies)
            {
                if (!helper.Contains(dependency))
                {
                    var p = all.FirstOrDefault(x => x.packageId.Equals(dependency));
                    if (p != null)
                    {
                        p.Install();
                    }
                }
            }
        }
    }
}
