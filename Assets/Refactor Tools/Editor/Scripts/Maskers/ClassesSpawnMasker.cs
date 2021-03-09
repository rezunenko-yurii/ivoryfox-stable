using UnityEditor;
using UnityEngine;

namespace IvoryFox_Engine.Release_Tools.Refactor_Tools.Editor.Scripts.Maskers
{
    public partial class AssetsMaskerWindow
    {
        private int amount = 10;
        private float def_labelWidth = EditorGUIUtility.labelWidth;
        private void ClassSpawnerBlock()
        {
            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField("SPAWN CLASSES", headerStyle);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Spawn"))
            {
                var spawner = new ClassSpawner();
                spawner.Spawn(amount);
            }

            EditorGUIUtility.labelWidth = 50f;
            amount = EditorGUILayout.IntField("amount", amount, GUILayout.ExpandWidth(false));
            EditorGUIUtility.labelWidth = def_labelWidth;
            
            GUILayout.EndHorizontal();
        }
    }
}