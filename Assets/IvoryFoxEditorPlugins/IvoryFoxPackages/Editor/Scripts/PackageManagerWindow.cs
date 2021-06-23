using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    public class PackageManagerWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Packages/Package Manager Window")]
        public static void ShowWindow() => GetWindow<PackageManagerWindow>("Package Manager");

        private Package[] all;
        private void Awake()
        {
            all = Resources.LoadAll<Package>("");
            foreach (var package in all)
            {
                package.PreparePackages();
            }
        }

        private void OnGUI()
        {
            var manifest = ManifestHelper.GetInstance();
        
            EditorGUILayout.BeginScrollView(new Vector2(100,400));
        
            foreach (Package package in all)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(package.packageName);

                /*if (package.packageAsset != null)
                {
                    GUILayout.Label("nan/nan");
                }*/
                string version = string.Empty;

                if (package.localPackage is null)
                {
                    version += "null / ";
                }
                else
                {
                    version += $"{package.localPackage.version} / ";
                }
                
                if (package.gitPackage is null)
                {
                    version += "null";
                }
                else
                {
                    version += $"{package.gitPackage.version}";
                }
                
                GUILayout.Label(version);
            
                if (manifest.Contains(package.packageId))
                {
                    GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("Update"))
                    {
                        package.InstallOrUpdate();
                    }
                
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove"))
                    {
                        package.Remove();
                    }
                }
                else
                {
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Install"))
                    {
                        package.InstallOrUpdate();
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
            }
        
            EditorGUILayout.EndScrollView();
        }
    }
}
