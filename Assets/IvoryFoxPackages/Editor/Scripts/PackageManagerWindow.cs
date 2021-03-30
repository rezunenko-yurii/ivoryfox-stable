using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    public class PackageManagerWindow : EditorWindow
    {
        [MenuItem("IvoryFox/Package Manager")]
        public static void ShowWindow() => GetWindow<PackageManagerWindow>("Package Manager");

        private Package[] all;
        private void Awake()
        {
            all = Resources.LoadAll<Package>("");
        }

        private void OnGUI()
        {
            var manifest = ManifestHelper.GetInstance();
        
            EditorGUILayout.BeginScrollView(new Vector2(100,400));
        
            foreach (Package package in all)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(package.packageName);
            
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
