using UnityEditor;
using UnityEngine;

namespace IvoryFoxPackages.Editor.Scripts
{
    [CustomEditor(typeof(Package))]
    public class PackageEditor : UnityEditor.Editor
    {
        /*public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Package package = (Package)target;

            if (ManifestHelper.GetInstance().Contains(package.packageId))
            {
                GUI.backgroundColor = Color.red;
                if(GUILayout.Button("Remove"))
                {
                    package.Remove();
                } 
            }
            else
            {
                GUI.backgroundColor = Color.green;
                if(GUILayout.Button("Install"))
                {
                    package.InstallOrUpdate();
                }
            }
            
            GUI.backgroundColor = Color.yellow;
            if(GUILayout.Button("Install Unity Packages"))
            {
                package.InstallUnityPackages();
            }
        }*/
    }
}
