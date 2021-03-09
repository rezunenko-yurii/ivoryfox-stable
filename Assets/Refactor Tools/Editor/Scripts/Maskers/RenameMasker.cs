using System.Collections.Generic;
using RedBlueGames.MulliganRenamer;
using UnityEditor;
using UnityEngine;

namespace IvoryFox_Engine.Release_Tools.Refactor_Tools.Editor.Scripts.Maskers
{
    public partial class AssetsMaskerWindow
    {
        private void RenamerSearchBlock()
        {
            EditorGUILayout.Space(10f);
            GUILayout.Label("FIND ASSETS AND DROP TO RENAMER", headerStyle);

            if (GUILayout.Button("Find sprites"))
            {
                IEnumerable<string> files = FindAll(new[] {".png", ".jpeg"});
                DropToRenamer(files);
            }

            if (GUILayout.Button("Find sounds"))
            {
                IEnumerable<string> files = FindAll(new[] {".mp3", ".ogg", ".wav"});
                DropToRenamer(files);
            }

            if (GUILayout.Button("Find prefabs"))
            {
                IEnumerable<string> files = FindAll( new[] {".prefab"});
                DropToRenamer(files);
            }
        }
        
        private void DropToRenamer(IEnumerable<string> files)
        {
            List<Object> objects = new List<Object>();

            foreach (string sFilePath in files)
            {
                string path = sFilePath.Replace("/", "\\");
                Object objAsset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                objects.Add(objAsset);
                Debug.Log(path);
                Debug.Log(objAsset.name);
            }
            
            Debug.Log($"Found {objects.Count} assets");
            
            if (objects.Count > 0)
            {
                MulliganRenamerPreviewPanel.AddObject.Invoke(objects);
            }
        }
    }
}